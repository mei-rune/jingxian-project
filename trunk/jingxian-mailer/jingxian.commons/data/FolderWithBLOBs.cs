namespace jingxian.data
{
    using jingxian.collections;

    public class FolderWithBLOBs : Folder
    {
        private Properties _misc;

        public Properties Misc
        {
            get 
            {
                if (null == _misc)
                    _misc = new Properties();
                return _misc; 
            }
            set { _misc = value; }
        }
    }
}