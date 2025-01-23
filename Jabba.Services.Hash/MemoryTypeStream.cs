namespace Jabba.Services.Hash
{
    internal class MemoryTypeStream : Stream
    {
        private readonly ReadOnlyMemory<byte> _data;
        private int _position;

        public MemoryTypeStream(ReadOnlyMemory<byte> data)
        {
            _data = data;
            _position = 0;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            
            if (offset < 0 || count < 0 || offset + count > buffer.Length)
                throw new ArgumentOutOfRangeException();

            int bytesAvailable = _data.Length - _position;
            
            if (bytesAvailable <= 0)
                return 0;

            int bytesToRead = Math.Min(count, bytesAvailable);
            var span = _data.Span.Slice(_position, bytesToRead);
            span.CopyTo(buffer.AsSpan(offset, bytesToRead));

            _position += bytesToRead;
            return bytesToRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long newPosition = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => _position + offset,
                SeekOrigin.End => _data.Length + offset,
                _ => throw new ArgumentOutOfRangeException(nameof(origin), "Invalid seek origin.")
            };

            if (newPosition < 0 || newPosition > _data.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), "Seek position is out of bounds.");

            _position = (int)newPosition;
            return _position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("Cannot set length on a read-only stream.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Cannot write to a read-only stream.");
        }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;

        public override long Length => _data.Length;

        public override long Position
        {
            get => _position;
            set
            {
                if (value < 0 || value > _data.Length)
                    throw new ArgumentOutOfRangeException(nameof(value), "Position is out of bounds.");
                _position = (int)value;
            }
        }
    }
}
