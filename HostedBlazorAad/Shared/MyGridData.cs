using System;

namespace HostedBlazorAad.Shared;

public class MyGridData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Colour { get; set; } = Colours.BLUE;
}

public static class Colours
{
    public const string BLUE = "blue";
    public const string RED = "red";
    public const string YELLOW = "yellow";
    public const string ORANGE = "orange";
    public const string GREEN = "green";
    public const string WHITE = "white";
}

public static class MyData
{
    private static readonly MyGridData[] _mydata = new MyGridData[]
    {
        new MyGridData { Id = 1, Name = "Liam", Colour = Colours.GREEN},
        new MyGridData { Id = 2, Name = "Michael", Colour = Colours.RED},
        new MyGridData { Id = 3, Name = "Thomas", Colour = Colours.BLUE},
        new MyGridData { Id = 4, Name = "Gráinne", Colour = Colours.WHITE},
        new MyGridData { Id = 5, Name = "James", Colour = Colours.YELLOW},
        new MyGridData { Id = 6, Name = "Noel", Colour = Colours.ORANGE},
        new MyGridData { Id = 7, Name = "Aisling", Colour = Colours.RED},
        new MyGridData { Id = 8, Name = "Derek", Colour = Colours.GREEN},
        new MyGridData { Id = 9, Name = "Fergal", Colour = Colours.BLUE},
        new MyGridData { Id = 10, Name = "Niall", Colour = Colours.YELLOW},
        new MyGridData { Id = 11, Name = "Eoin", Colour = Colours.ORANGE},
        new MyGridData { Id = 12, Name = "Séan", Colour = Colours.GREEN},
        new MyGridData { Id = 13, Name = "Deirdre", Colour = Colours.RED},
        new MyGridData { Id = 14, Name = "Nuala", Colour = Colours.YELLOW},
        new MyGridData { Id = 15, Name = "Lorna", Colour = Colours.BLUE},
        new MyGridData { Id = 16, Name = "Linda", Colour = Colours.WHITE},
        new MyGridData { Id = 17, Name = "Eileen", Colour = Colours.ORANGE},
        new MyGridData { Id = 18, Name = "Siobhain", Colour = Colours.RED},
        new MyGridData { Id = 19, Name = "Fiona", Colour = Colours.ORANGE},
        new MyGridData { Id = 20, Name = "Maeve", Colour = Colours.GREEN},
        new MyGridData { Id = 21, Name = "Damien", Colour = Colours.GREEN},
        new MyGridData { Id = 22, Name = "Niamh", Colour = Colours.YELLOW},
        new MyGridData { Id = 23, Name = "Louise", Colour = Colours.BLUE},
        new MyGridData { Id = 24, Name = "Linda", Colour = Colours.WHITE},
        new MyGridData { Id = 25, Name = "Therese", Colour = Colours.ORANGE},
        new MyGridData { Id = 26, Name = "Clodagh", Colour = Colours.RED},
        new MyGridData { Id = 27, Name = "Aidan", Colour = Colours.BLUE},
        new MyGridData { Id = 28, Name = "Cathal ", Colour = Colours.YELLOW},
        new MyGridData { Id = 29, Name = "Donnacha ", Colour = Colours.ORANGE},
        new MyGridData { Id = 30, Name = "Shay", Colour = Colours.GREEN},
        new MyGridData { Id = 31, Name = "Colm ", Colour = Colours.WHITE},
        new MyGridData { Id = 32, Name = "Daithi", Colour = Colours.YELLOW},
        new MyGridData { Id = 33, Name = "Patrick", Colour = Colours.ORANGE},
    };

    public static ReadOnlySpan<MyGridData> GetData()
    {
        return _mydata.AsSpan();
    }
}
