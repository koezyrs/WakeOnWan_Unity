using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WakeOnWanCore : MonoBehaviour
{
    [SerializeField] private TMP_InputField macAddress;
    [SerializeField] private TMP_InputField ipAddress;
    [SerializeField] private TMP_InputField port;

    public void SendMagicPacket()
    {
        string macAddress = this.macAddress.text;
        string ipAddress = this.ipAddress.text;
        int port = 9;

        if (string.IsNullOrEmpty(macAddress) || macAddress.Contains("\u200B"))
        {
            PopupMessage.Instance.ShowMessage("MAC Address is empty!");
            return;
        }
        
        
        if (string.IsNullOrEmpty(ipAddress) || macAddress.Contains("\u200B"))
        {
            PopupMessage.Instance.ShowMessage("IP Address or Host Name is empty!");
            return;
        }
        
        bool ok = int.TryParse(this.port.text, out port);
        if (!ok || port < 0 || port > 65535)
        {
            PopupMessage.Instance.ShowMessage("Port invalid!");
            return;
        }
        
        try
        {
            // Convert the MAC address to bytes
            byte[] macBytes = ParseMacAddress(macAddress);

            // Create the magic packet
            byte[] packet = CreateMagicPacket(macBytes);

            // Send the packet via UDP
            using (UdpClient udpClient = new UdpClient())
            {
                udpClient.Connect(ipAddress, port);
                udpClient.Send(packet, packet.Length);
                PopupMessage.Instance.ShowMessage($"Magic packet sent to {macAddress} at {ipAddress}");
                Debug.Log($"Magic packet sent to {macAddress} at {ipAddress}");
            }
        }
        catch (Exception ex)
        {
            PopupMessage.Instance.ShowMessage("Error",$"Error sending magic packet: {ex.Message}");
            Debug.LogError($"Error sending magic packet: {ex.Message}");
        }
    }

    private byte[] ParseMacAddress(string macAddress)
    {
        string[] macParts = macAddress.Split(':', '-');
        byte[] macBytes = new byte[macParts.Length];
        for (int i = 0; i < macParts.Length; i++)
        {
            macBytes[i] = Convert.ToByte(macParts[i], 16);
        }
        return macBytes;
    }

    private byte[] CreateMagicPacket(byte[] macBytes)
    {
        byte[] packet = new byte[6 + (macBytes.Length * 16)];
        // Fill the first 6 bytes with 0xFF
        for (int i = 0; i < 6; i++)
        {
            packet[i] = 0xFF;
        }
        // Fill the rest with the MAC address repeated 16 times
        for (int i = 6; i < packet.Length; i += macBytes.Length)
        {
            Buffer.BlockCopy(macBytes, 0, packet, i, macBytes.Length);
        }
        return packet;
    }
}
