using System;
namespace Domain.Musics
{
    /// <summary>
    /// フォルテッシモ用メモリストリームラッパークラス <br/>
    /// エンコード済み音楽データを一時的に溜め込む <br/>
    /// 書き込みの方が早いことが前提
    /// </summary>
    public class MemoryStreamWrapper : IDisposable
    {
        private readonly MemoryStream _memoryStream;

        private long _readPosition;

        private long _writePosition;

        private bool _isWrote;

        public MemoryStreamWrapper()
        {
            this._memoryStream = new MemoryStream();
            this._isWrote = false;
        }

        /// <summary>
        /// データ読み込みメソッド
        /// </summary>
        /// <param name="buffer">読み込んだデータの格納先</param>
        /// <param name="offset">格納先への書き込み開始位置</param>
        /// <param name="count">読み込むデータ量</param>
        /// <returns></returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            while (this._writePosition - this._readPosition < count - offset && !this._isWrote)
            {
                // 読み可能になるまで待機
                Task.Delay(50);
            }
            int num;
            lock (this._memoryStream)
            {
                this._memoryStream.Position = this._readPosition;
                num = this._memoryStream.Read(buffer, offset, count);
            }
            this._readPosition += num;
            return num;
        }

        /// <summary>
        /// データ書き込みメソッド
        /// </summary>
        /// <param name="buffer">書き込むデータ</param>
        /// <param name="offset">書き込むデータ内の読み込み開始位置</param>
        /// <param name="count">書き込むデータ量</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            lock (this._memoryStream)
            {
                this._memoryStream.Position = this._writePosition;
                this._memoryStream.Write(buffer, offset, count);
            }
            this._writePosition += (count - offset);
        }

        /// <summary>
        /// 書き込み完了メソッド
        /// </summary>
        public void WriteFinish()
        {
            this._isWrote = true;
        }

        public void Dispose()
        {
            this._memoryStream.Dispose();
        }

    }
}

