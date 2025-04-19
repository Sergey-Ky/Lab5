using System.Xml;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
//---------------------------------------------XML--------------------------------------------------

Shape[] shapes = new Shape[4];
shapes[0] = new Square();
shapes[1] = new Triangle();
shapes[2] = new EquilateralTriangle();
shapes[3] = new RightTriangle();
XmlWriterSettings settings = new XmlWriterSettings
{
    Indent = true,
    IndentChars = "    ",
    NewLineOnAttributes = false
};

// Создаем XmlWriter и записываем данные
using (XmlWriter writer = XmlWriter.Create("C:\\Users\\ser20\\RiderProjects\\Lab5\\Lab5\\shapes.xml", settings))
{
    writer.WriteStartDocument();
    writer.WriteStartElement("Shapes");

    foreach (Shape shape in shapes)
    {
        shape.SaveToXml(writer);
    }

    writer.WriteEndElement();
    writer.WriteEndDocument();
}


public interface IShape
{
    int Area();
    void DisplayProperties();
}

public abstract class Shape : IShape
{
    private static readonly Random random = new Random();

    protected static int GetRandomSide(int min = 1, int max = 20)
    {
        return random.Next(min, max + 1);
    }

    public abstract int Area();
    public abstract void DisplayProperties();
    public abstract void SaveToXml(XmlWriter writer);
    public abstract void LoadFromXml(XmlReader reader);
}

public abstract class Quadrilateral : Shape
{
    public int SideA;
    public int SideB;
    public int SideC;
    public int SideD;

    protected Quadrilateral()
    {
        SideA = GetRandomSide();
        SideB = GetRandomSide();
        SideC = GetRandomSide();
        SideD = GetRandomSide();
    }
}

public class Square : Quadrilateral
{
    public Square()
    {
        int side = GetRandomSide();
        SideA = SideB = SideC = SideD = side;
    }

    public override int Area()
    {
        return SideA * SideA;
    }

    public override void DisplayProperties()
    {
        Console.WriteLine($"Квадрат со стороной: {SideA}");
    }
    
    ~Square()
    {
        return;
    }
    
    public override void SaveToXml(XmlWriter writer)
    {
        writer.WriteStartElement("Square");
        writer.WriteAttributeString("Sides", $"{SideA}");
        writer.WriteEndElement();
    }

    public override void LoadFromXml(XmlReader reader)
    {
        SideA = SideB = SideC = SideD = int.Parse(reader.GetAttribute("Sides"));
    }
}

public class Triangle : Shape
{
    public int SideA;
    public int SideB;
    public int SideC;

    public Triangle()
    {
        do {
            SideA = GetRandomSide();
            SideB = GetRandomSide();
            SideC = GetRandomSide();
        } while (!IsValidTriangle(SideA, SideB, SideC));
    }

    public bool IsValidTriangle(int a, int b, int c)
    {
        return a + b > c && a + c > b && b + c > a;
    }

    public override int Area()
    {
        // Формула Герона (результат округляется до целого)
        double p = (SideA + SideB + SideC) / 2.0;
        double area = Math.Sqrt(p * (p - SideA) * (p - SideB) * (p - SideC));
        return (int)Math.Round(area);
    }

    public override void DisplayProperties()
    {
        Console.WriteLine($"Треугольник со сторонами: {SideA}, {SideB}, {SideC}");
    }

    ~Triangle()
    {
        return;
    }
    
    public override void SaveToXml(XmlWriter writer)
    {
        writer.WriteStartElement("Triangle");
        writer.WriteAttributeString("Sides", $"{SideA} {SideB} {SideC}");
        writer.WriteEndElement();
    }

    public override void LoadFromXml(XmlReader reader)
    {
        string[] sides = reader.GetAttribute("Sides").Split(' ');
        SideA = int.Parse(sides[0]);
        SideB = int.Parse(sides[1]);
        SideC = int.Parse(sides[2]);
    }
}

public class IsoscelesTriangle : Triangle
{
    protected IsoscelesTriangle(int equalSides, int baseSide)
    {
        SideA = SideB = equalSides;
        SideC = baseSide;
    }

    public override void DisplayProperties()
    {
        Console.WriteLine($"Равнобедренный треугольник: равные стороны {SideA}, основание {SideC}");
    }

    ~IsoscelesTriangle()
    {
        return;
    }
    
    public override void SaveToXml(XmlWriter writer)
    {
        writer.WriteStartElement("IsoscelesTriangle");
        writer.WriteAttributeString("Sides", $"{SideA} {SideB} {SideC}");
        writer.WriteEndElement();
    }

    public override void LoadFromXml(XmlReader reader)
    {
        string[] sides = reader.GetAttribute("Sides").Split(' ');
        SideA = int.Parse(sides[0]);
        SideB = int.Parse(sides[1]);
        SideC = int.Parse(sides[2]);
    }
}

public class EquilateralTriangle : IsoscelesTriangle
{
    public EquilateralTriangle() : base(GetRandomSide(), GetRandomSide())
    {
        SideC = SideA;
    }

    public override int Area()
    {
        double area = (Math.Sqrt(3) / 4) * SideA * SideA;
        return (int)Math.Round(area);
    }

    public override void DisplayProperties()
    {
        Console.WriteLine($"Равносторонний треугольник со стороной: {SideA}");
    }

    ~EquilateralTriangle()
    {
        return;
    }
    
    public override void SaveToXml(XmlWriter writer)
    {
        writer.WriteStartElement("EquilateralTriangle");
        writer.WriteAttributeString("Sides", $"{SideA} {SideB} {SideC}");
        writer.WriteEndElement();
    }

    public override void LoadFromXml(XmlReader reader)
    {
        string[] sides = reader.GetAttribute("Sides").Split(' ');
        SideA = int.Parse(sides[0]);
        SideB = int.Parse(sides[1]);
        SideC = int.Parse(sides[2]);
    }
}

public class RightTriangle : Triangle
{
    public RightTriangle()
    {
        do
        {
            SideA = GetRandomSide();
            SideB = GetRandomSide();
            SideC = (int)Math.Round(Math.Sqrt(SideA * SideA + SideB * SideB));
        } while (!IsValidTriangle(SideA, SideB, SideC));
    }

    public override int Area()
    {
        return (SideA * SideB) / 2;
    }

    public override void DisplayProperties()
    {
        Console.WriteLine($"Прямоугольный треугольник с катетами: {SideA}, {SideB} и гипотенузой: {SideC}");
    }

    ~RightTriangle()
    {
        return;
    }
    public override void SaveToXml(XmlWriter writer)
    {
        writer.WriteStartElement("RightTriangle");
        writer.WriteAttributeString("Sides", $"{SideA} {SideB} {SideC}");
        writer.WriteEndElement();
    }

    public override void LoadFromXml(XmlReader reader)
    {
        string[] sides = reader.GetAttribute("Sides").Split(' ');
        SideA = int.Parse(sides[0]);
        SideB = int.Parse(sides[1]);
        SideC = int.Parse(sides[2]);
    }
}
// ---------------------------JSON--------------------------------

// Shape[] shapes = new Shape[4];
// shapes[0] = new Square();
// shapes[1] = new Triangle();
// shapes[2] = new EquilateralTriangle();
// shapes[3] = new RightTriangle();
//
// WriteJsonElementsToFile(shapes, "C:\\Users\\ser20\\RiderProjects\\Lab5\\Lab5\\shapes.json");
//
//
// static void WriteJsonElementsToFile(Shape[] elements, string filePath)
// {
//     var jsonArray = new JsonArray();
//     foreach (var element in elements)
//     {
//         jsonArray.Add(JsonNode.Parse(element.ToJson().GetRawText()));
//     }
//     var options = new JsonSerializerOptions 
//     { 
//         WriteIndented = true,
//         Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
//     };
//     File.WriteAllText(filePath, jsonArray.ToJsonString(options));
// }
//
// public interface IShape
// {
//     int Area();
//     void DisplayProperties();
// }
//
// public abstract class Shape : IShape
// {
//     private static readonly Random random = new Random();
//
//     protected static int GetRandomSide(int min = 1, int max = 20)
//     {
//         return random.Next(min, max + 1);
//     }
//
//     public abstract int Area();
//     public abstract void DisplayProperties();
//     public abstract JsonElement ToJson();
//     public abstract void FromJson(JsonElement element);
// }
//
// public abstract class Quadrilateral : Shape
// {
//     public int SideA;
//     public int SideB;
//     public int SideC;
//     public int SideD;
//
//     protected Quadrilateral()
//     {
//         SideA = GetRandomSide();
//         SideB = GetRandomSide();
//         SideC = GetRandomSide();
//         SideD = GetRandomSide();
//     }
// }
//
// public class Square : Quadrilateral
// {
//     public Square()
//     {
//         int side = GetRandomSide();
//         SideA = SideB = SideC = SideD = side;
//     }
//
//     public override int Area()
//     {
//         return SideA * SideA;
//     }
//
//     public override void DisplayProperties()
//     {
//         Console.WriteLine($"Квадрат со стороной: {SideA}");
//     }
//     
//     ~Square()
//     {
//         return;
//     }
//     
//     public override JsonElement ToJson()
//     {
//         return JsonSerializer.SerializeToElement(new
//         {
//             Type = "Square",
//             Sides = SideA,
//         });
//     }
//
//     public override void FromJson(JsonElement element)
//     {
//         SideA = SideB = SideC = SideD = int.Parse(element.GetProperty("Sides").GetString());
//     }
// }
//
// public class Triangle : Shape
// {
//     public int SideA;
//     public int SideB;
//     public int SideC;
//
//     public Triangle()
//     {
//         do {
//             SideA = GetRandomSide();
//             SideB = GetRandomSide();
//             SideC = GetRandomSide();
//         } while (!IsValidTriangle(SideA, SideB, SideC));
//     }
//
//     public bool IsValidTriangle(int a, int b, int c)
//     {
//         return a + b > c && a + c > b && b + c > a;
//     }
//
//     public override int Area()
//     {
//         // Формула Герона (результат округляется до целого)
//         double p = (SideA + SideB + SideC) / 2.0;
//         double area = Math.Sqrt(p * (p - SideA) * (p - SideB) * (p - SideC));
//         return (int)Math.Round(area);
//     }
//
//     public override void DisplayProperties()
//     {
//         Console.WriteLine($"Треугольник со сторонами: {SideA}, {SideB}, {SideC}");
//     }
//
//     ~Triangle()
//     {
//         return;
//     }
//     
//     public override JsonElement ToJson()
//     {
//         return JsonSerializer.SerializeToElement(new
//         {
//             Type = "Triangle",
//             Sides = $"{SideA} {SideB} {SideC}",
//         });
//     }
//
//     public override void FromJson(JsonElement element)
//     {
//         string[] sides = element.GetProperty("Sides").GetString().Split(' ');
//         SideA = int.Parse(sides[0]);
//         SideB = int.Parse(sides[1]);
//         SideC = int.Parse(sides[2]);;
//     }
// }
//
// public class IsoscelesTriangle : Triangle
// {
//     protected IsoscelesTriangle(int equalSides, int baseSide)
//     {
//         SideA = SideB = equalSides;
//         SideC = baseSide;
//     }
//
//     public override void DisplayProperties()
//     {
//         Console.WriteLine($"Равнобедренный треугольник: равные стороны {SideA}, основание {SideC}");
//     }
//
//     ~IsoscelesTriangle()
//     {
//         return;
//     }
//     
//     public override JsonElement ToJson()
//     {
//         return JsonSerializer.SerializeToElement(new
//         {
//             Type = "IsoscelesTriangle",
//             Sides = $"{SideA} {SideB} {SideC}",
//         });
//     }
//
//     public override void FromJson(JsonElement element)
//     {
// string[] sides = element.GetProperty("Sides").GetString().Split(' ');
//         SideA = int.Parse(sides[0]);
//         SideB = int.Parse(sides[1]);
//         SideC = int.Parse(sides[2]);;
//     }
// }
//
// public class EquilateralTriangle : IsoscelesTriangle
// {
//     public EquilateralTriangle() : base(GetRandomSide(), GetRandomSide())
//     {
//         SideC = SideA;
//     }
//
//     public override int Area()
//     {
//         double area = (Math.Sqrt(3) / 4) * SideA * SideA;
//         return (int)Math.Round(area);
//     }
//
//     public override void DisplayProperties()
//     {
//         Console.WriteLine($"Равносторонний треугольник со стороной: {SideA}");
//     }
//
//     ~EquilateralTriangle()
//     {
//         return;
//     }
//     
//     public override JsonElement ToJson()
//     {
//         return JsonSerializer.SerializeToElement(new
//         {
//             Type = "EquilateralTriangle",
//             Sides = $"{SideA} {SideB} {SideC}",
//         });
//     }
//
//     public override void FromJson(JsonElement element)
//     {
//         string[] sides = element.GetProperty("Sides").GetString().Split(' ');
//         SideA = int.Parse(sides[0]);
//         SideB = int.Parse(sides[1]);
//         SideC = int.Parse(sides[2]);;
//     }
// }
//
// public class RightTriangle : Triangle
// {
//     public RightTriangle()
//     {
//         do
//         {
//             SideA = GetRandomSide();
//             SideB = GetRandomSide();
//             SideC = (int)Math.Round(Math.Sqrt(SideA * SideA + SideB * SideB));
//         } while (!IsValidTriangle(SideA, SideB, SideC));
//     }
//
//     public override int Area()
//     {
//         return (SideA * SideB) / 2;
//     }
//
//     public override void DisplayProperties()
//     {
//         Console.WriteLine($"Прямоугольный треугольник с катетами: {SideA}, {SideB} и гипотенузой: {SideC}");
//     }
//
//     ~RightTriangle()
//     {
//         return;
//     }
//     public override JsonElement ToJson()
//     {
//         return JsonSerializer.SerializeToElement(new
//         {
//             Type = "RightTriangle",
//             Sides = $"{SideA} {SideB} {SideC}",
//         });
//     }
//
//     public override void FromJson(JsonElement element)
//     {
//         string[] sides = element.GetProperty("Sides").GetString().Split(' ');
//         SideA = int.Parse(sides[0]);
//         SideB = int.Parse(sides[1]);
//         SideC = int.Parse(sides[2]);;
//     }
// }
//
