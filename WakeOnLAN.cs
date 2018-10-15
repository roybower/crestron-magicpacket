using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;
using Crestron.SimplSharpPro;


namespace MagicPacket
{
    public class WakeOnLAN
    {
        public void WakeUp(byte[] mac)
        {
            try
            {
                SocketErrorCodes errCode;

                UDPServer WOL = new UDPServer();

                // enable UDP server
                errCode = WOL.EnableUDPServer(IPAddress.Any, 0, 9);
                if (errCode != SocketErrorCodes.SOCKET_OK)
                    CrestronConsole.PrintLine("UDP EnableUDPServer() result: {0}", errCode);

                WOL.EthernetAdapterToBindTo = EthernetAdapterType.EthernetLANAdapter;

                byte[] packet = new byte[102]; // 17 * 6

                for (int i = 0; i < 6; i++)
                {
                    packet[i] = 0xFF;
                }

                for (int i = 1; i <= 16; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        packet[i * 6 + j] = mac[j];
                    }
                }

                for (int i = 0; i < 3; ++i)
                {
                    errCode = WOL.SendData(packet, packet.Length, "192.168.1.255", 9, false);
                    if (errCode != SocketErrorCodes.SOCKET_OK)
                        CrestronConsole.PrintLine("UDP SendData() result: {0}", errCode);
                }

                string pckt = BitConverter.ToString(packet);
                CrestronConsole.PrintLine(pckt);

                WOL.DisableUDPServer();
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("WakeOnLAN() exception {0} ", ex);
            }
        } 

    }
}