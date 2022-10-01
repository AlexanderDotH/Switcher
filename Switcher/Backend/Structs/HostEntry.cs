using System;

namespace Switcher.Backend.Structs;

public class HostEntry
{
    private string _ipV4p1;
    private string _ipV4p2;
    private string _ipV6p1;
    private string _ipV6p2;

    public HostEntry(string ipV4P1, string ipV4P2, string ipV6P1, string ipV6P2)
    {
        this._ipV4p1 = ipV4P1;
        this._ipV4p2 = ipV4P2;
        this._ipV6p1 = ipV6P1;
        this._ipV6p2 = ipV6P2;
    }

    public string IpV4P1
    {
        get => _ipV4p1;
        set => _ipV4p1 = value;
    }

    public string IpV4P2
    {
        get => _ipV4p2;
        set => _ipV4p2 = value;
    }

    public string IpV6P1
    {
        get => _ipV6p1;
        set => _ipV6p1 = value;
    }

    public string IpV6P2
    {
        get => _ipV6p2;
        set => _ipV6p2 = value;
    }
}