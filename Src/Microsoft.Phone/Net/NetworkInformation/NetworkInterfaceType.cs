using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.Net.NetworkInformation
{
    // Enumeration representing different types of network interfaces
    public enum NetworkInterfaceType
    {
        // No network interface is present or active
        None = 0,

        // The interface type is not recognized
        Unknown = 1,

        // Ethernet (IEEE 802.3 standard)
        Ethernet = 6,

        // Token Ring (IEEE 802.5 standard)
        TokenRing = 9,

        // Fiber Distributed Data Interface
        Fddi = 15,

        // Basic ISDN connection
        BasicIsdn = 20,

        // Primary ISDN connection
        PrimaryIsdn = 21,

        // Point-to-Point Protocol
        Ppp = 23,

        // Loopback interface
        Loopback = 24,

        // 3 Mbps Ethernet
        Ethernet3Megabit = 26,

        // Serial Line Internet Protocol
        Slip = 28,

        // Asynchronous Transfer Mode
        Atm = 37,

        // Generic modem interface
        GenericModem = 48,

        // Fast Ethernet (100Base-T)
        FastEthernetT = 62,

        // ISDN interface
        Isdn = 63,

        // Fast Ethernet over fiber (100Base-FX)
        FastEthernetFx = 69,

        // Wireless (IEEE 802.11)
        Wireless80211 = 71,

        // Asymmetric DSL
        AsymmetricDsl = 94,

        // Rate-Adaptive DSL
        RateAdaptDsl = 95,

        // Symmetric DSL
        SymmetricDsl = 96,

        // Very High Speed DSL
        VeryHighSpeedDsl = 97,

        // IP over ATM
        IPOverAtm = 114,

        // Gigabit Ethernet
        GigabitEthernet = 117,

        // Tunnel interface
        Tunnel = 131,

        // Multi-rate Symmetric DSL
        MultiRateSymmetricDsl = 143,

        // High-Speed Serial Bus (e.g., FireWire)
        HighPerformanceSerialBus = 144,

        // Mobile Broadband (GSM)
        MobileBroadbandGsm = 145,

        // Mobile Broadband (CDMA)
        MobileBroadbandCdma = 146
    }
}