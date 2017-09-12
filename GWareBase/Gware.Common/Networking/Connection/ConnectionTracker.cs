using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Connection
{
    public struct ConnectionTracker
    {
        private const ushort c_halfSequenceMax = ushort.MaxValue / 2;
        public ushort RemoteSequence { get; private set; }
        public uint Ack { get; private set; }
        public IPEndPoint EndPoint { get; private set; }

        public ConnectionTracker(IPEndPoint endPoint,ushort sequenceStart)
        {
            RemoteSequence = sequenceStart;
            Ack = 1;
            EndPoint = endPoint;
        }

        public ConnectionTracker(IPEndPoint endPoint)
        {
            RemoteSequence = 0;
            Ack = 0;
            EndPoint = endPoint;
        }

        public void UpdateRemoteSequence(ushort remoteSequence)
        {
            bool greaterSequence = IsSequenceGreaterThan(remoteSequence, RemoteSequence, out bool overflow);
            int difference = SequenceDifference(remoteSequence, RemoteSequence,greaterSequence, overflow);
            if (!greaterSequence)
            {
                if (difference < (sizeof(uint) * 8))
                {
                    Ack |= (uint)Math.Pow(2, difference);
                }
            }
            else
            {
                if(difference > sizeof(uint) * 8)
                {
                    Ack = 0;
                }
                else
                {
                    Ack = Ack << difference;
                }
                
                Ack |= 0x01;

                RemoteSequence = remoteSequence;
            }
        }
        private int SequenceDifference(ushort nextValue, ushort previousValue,bool greater,bool overflow)
        {
            ushort maxValue = Math.Max(previousValue, nextValue);
            ushort minValue = Math.Min(previousValue, nextValue);

            if (overflow)
            {
                return ((ushort.MaxValue - maxValue) + minValue) + 1;
            }
            else
            {
                return maxValue - minValue;
            }          
            
        }
        private bool IsSequenceGreaterThan(ushort nextValue, ushort previousValue, out bool overflow)
        {
            bool forwardOverflow = (nextValue < previousValue) && (previousValue - nextValue > c_halfSequenceMax);
            overflow = ((nextValue > previousValue) && nextValue - previousValue > c_halfSequenceMax) || forwardOverflow;
            return forwardOverflow || ((nextValue > previousValue) && (nextValue - previousValue <= c_halfSequenceMax));
        }
    }
}