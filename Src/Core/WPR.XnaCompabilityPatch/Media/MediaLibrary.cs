using System;
using System.Collections.Generic;

namespace WPR.XnaCompability.Media
{
    public class MediaLibrary
    {
        private SongCollection _Songs;
        private ArtistCollection _Artists;
        private AlbumCollection _Albums;
        private PictureCollection _Pictures;
        private PlaylistCollection _Playlists;

        public MediaLibrary()
            : this(MediaSource.GetAvailableMediaSources()[0])
        {
        }

        public MediaLibrary(MediaSource source)
        {
            _Songs = new SongCollection();
            _Artists = new ArtistCollection();
            _Albums = new AlbumCollection();
            _Pictures = new PictureCollection();
            _Playlists = new PlaylistCollection();
        }

        public SongCollection Songs => _Songs;
        public ArtistCollection Artists => _Artists;
        public AlbumCollection Albums => _Albums;
        public PictureCollection Pictures => _Pictures;
        public PlaylistCollection Playlists => _Playlists;
    }
}
