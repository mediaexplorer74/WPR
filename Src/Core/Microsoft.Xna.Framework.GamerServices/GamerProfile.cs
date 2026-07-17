using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
using System.IO;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class GamerProfile : IDisposable
    {
        private static readonly byte[] PlaceholderGamerPicture =
        [
            0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,
            0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
            0x08, 0x06, 0x00, 0x00, 0x00, 0x1F, 0x15, 0xC4,
            0x89, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x44, 0x41,
            0x54, 0x08, 0xD7, 0x63, 0x60, 0x60, 0x60, 0x60,
            0x00, 0x00, 0x00, 0x05, 0x00, 0x01, 0xE2, 0x26,
            0x05, 0x9B, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45,
            0x4E, 0x44, 0xAE, 0x42, 0x60, 0x82
        ];

        internal GamerProfile()
        {
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public Texture2D GamerPicture
        {
            get;
            internal set;
        }

        public Stream GetGamerPicture() =>
            new MemoryStream(PlaceholderGamerPicture, writable: false);

        public int GamerScore
        {
            get;
            internal set;
        }

        public GamerZone GamerZone
        {
            get;
            internal set;
        }

        public bool IsDisposed
        {
            get;
            internal set;
        }

        public string Motto
        {
            get;
            internal set;
        }

        public RegionInfo Region
        {
            get;
            internal set;
        }

        public float Reputation
        {
            get;
            internal set;
        }

        public int TitlesPlayed
        {
            get;
            internal set;
        }

        public int TotalAchievements
        {
            get;
            internal set;
        }

    }
}
