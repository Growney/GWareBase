using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Connection
{
    public class ConnectionTracker
    {
        private const ushort c_halfSequenceMax = ushort.MaxValue / 2;
        public ushort Sequence { get; private set; }
        public uint Ack { get; private set; }
        public IPEndPoint EndPoint { get; private set; }

        public ConnectionTracker(IPEndPoint endPoint,ushort sequenceStart)
        {
            Sequence = sequenceStart;
            Ack = 0;
            EndPoint = endPoint;
        }

        public ConnectionTracker(IPEndPoint endPoint)
        {
            Sequence = 0;
            Ack = 0;
            EndPoint = endPoint;
        }

        public ConnectionTracker()
        {
            Sequence = 0;
            Ack = 0;
            EndPoint = null;
        }

        public ushort GetNextSequence()
        {
            ushort retVal = unchecked((ushort)(Sequence + 1));
            Ack  = Ack << 1;
            Sequence = retVal;
            return retVal;
        }

        public void AckSequence(ushort sequence,uint ack)
        {
            int difference = SequenceDifference(Sequence, sequence,out bool greater);
            if(difference  == 0 || (difference < sizeof(uint) * 8 && greater))
            {
                Ack |= ack << difference;
            }
        }

        public void UpdateRemoteSequence(ushort remoteSequence)
        {
            bool greaterSequence = IsSequenceGreaterThan(remoteSequence, Sequence, out bool overflow);
            int difference = SequenceDifference(remoteSequence, Sequence,greaterSequence, overflow);
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

                Sequence = remoteSequence;
            }
        }
        private static int SequenceDifference(ushort next,ushort previousValue,out bool greaterSequence)
        {
            greaterSequence = IsSequenceGreaterThan(next, previousValue, out bool overflow);
            return SequenceDifference(next, previousValue, greaterSequence, overflow);
        }

        private static int SequenceDifference(ushort nextValue, ushort previousValue,bool greater,bool overflow)
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
        private static bool IsSequenceGreaterThan(ushort nextValue, ushort previousValue, out bool overflow)
        {
            bool forwardOverflow = (nextValue < previousValue) && (previousValue - nextValue > c_halfSequenceMax);
            overflow = ((nextValue > previousValue) && nextValue - previousValue > c_halfSequenceMax) || forwardOverflow;
            return forwardOverflow || ((nextValue > previousValue) && (nextValue - previousValue <= c_halfSequenceMax));
        }
    }
}