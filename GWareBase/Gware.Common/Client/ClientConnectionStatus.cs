namespace Gware.Common.Client
{
    public struct ClientConnectionStatus
    {
        private bool m_success;
        private int m_hops;

        public bool Success
        {
            get
            {
                return m_success;
            }

            set
            {
                m_success = value;
            }
        }

        public int Hops
        {
            get
            {
                return m_hops;
            }

            set
            {
                m_hops = value;
            }
        }

        public ClientConnectionStatus(bool sucess,int hops)
        {
            m_success = sucess;
            m_hops = hops;
        }
    }
}
