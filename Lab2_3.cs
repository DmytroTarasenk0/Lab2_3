/// Прямокутники, сторони || 0x(y). Можливо перемістити, змінити розмір. 
/// Побудова найменшого прямокутника з двох заданих і прямокутник-перетин
/// 1 метод для збереження об'єкту у JSON
/// 2 для відкриття та створення об'єкту.

using System;
using System.IO;
using Newtonsoft.Json;

namespace Lab2_3
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public class Rectangle
    {
        public Point BottomLeft { get; set; }
        public Point TopRight { get; set; }

        public Rectangle(Point bottomLeft, Point topRight)
        {
            BottomLeft = bottomLeft;
            TopRight = topRight;
        }

        public void Move(int deltaX, int deltaY)
        {
            BottomLeft.X += deltaX;
            BottomLeft.Y += deltaY;
            TopRight.X += deltaX;
            TopRight.Y += deltaY;
        }

        public void Resize(int newWidth, int newHeight)
        {
            TopRight.X = BottomLeft.X + newWidth;
            TopRight.Y = BottomLeft.Y + newHeight;
        }

        public Rectangle Combine(Rectangle other)
        {
            int minX = Math.Min(this.BottomLeft.X, other.BottomLeft.X);
            int minY = Math.Min(this.BottomLeft.Y, other.BottomLeft.Y);
            int maxX = Math.Max(this.TopRight.X, other.TopRight.X);
            int maxY = Math.Max(this.TopRight.Y, other.TopRight.Y);

            return new Rectangle(new Point(minX, minY), new Point(maxX, maxY));
        }

        public Rectangle Intersection(Rectangle other)
        {
            int minX = Math.Max(this.BottomLeft.X, other.BottomLeft.X);
            int minY = Math.Max(this.BottomLeft.Y, other.BottomLeft.Y);
            int maxX = Math.Min(this.TopRight.X, other.TopRight.X);
            int maxY = Math.Min(this.TopRight.Y, other.TopRight.Y);

            if (minX <= maxX && minY <= maxY)
            {
                return new Rectangle(new Point(minX, minY), new Point(maxX, maxY));
            }
            else
            {
                return new Rectangle(new Point(0, 0), new Point(0, 0));
            }
        }
        public override string ToString()
        {
            return $"Bottom Left: {BottomLeft}, Top Right: {TopRight}";
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("As always, 1, 2 - task. 'Exit' for exit");
            while (true)
            {
                string u_input;
                u_input = Console.ReadLine();

                if (u_input.ToLower() == "exit")
                {
                    break;
                }
                else if (u_input.ToLower() == "1")
                {
                    try
                    {
                        Rectangle rect1 = new Rectangle(new Point(0, 0), new Point(5, 5));
                        Rectangle rect2 = new Rectangle(new Point(2, 2), new Point(8, 8));

                        Console.WriteLine("Initial rectangles:");
                        Console.WriteLine("Rectangle 1: " + rect1);
                        Console.WriteLine("Rectangle 2: " + rect2);

                        rect1.Move(2, 2);
                        Console.WriteLine("\nAfter moving Rectangle 1:");
                        Console.WriteLine("Rectangle 1: " + rect1);

                        rect2.Resize(2, 2);
                        Console.WriteLine("\nAfter resizing Rectangle 2:");
                        Console.WriteLine("Rectangle 2: " + rect2);

                        Rectangle combinedRect = rect1.Combine(rect2);
                        Console.WriteLine("\nCombined rectangle:");
                        Console.WriteLine("Combined Rectangle: " + combinedRect);

                        Rectangle intersectionRect = rect1.Intersection(rect2);
                        Console.WriteLine("\nIntersection rectangle:");
                        Console.WriteLine("Intersection Rectangle: " + intersectionRect);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error. " + e.Message);
                    }
                }
                else if (u_input.ToLower() == "2")
                {
                    try
                    {
                        Rectangle rectangle = new Rectangle(new Point(0, 0), new Point(5, 5));

                        Save(rectangle, "D:\\Projects_VS\\Lab2_3\\rectangle.json");

                        Rectangle restoredRectangle = Load("D:\\Projects_VS\\Lab2_3\\rectangle.json");

                        Console.WriteLine("Restored Rectangle:");
                        Console.WriteLine($"Bottom Left: ({restoredRectangle.BottomLeft.X}, {restoredRectangle.BottomLeft.Y}), " +
                                          $"Top Right: ({restoredRectangle.TopRight.X}, {restoredRectangle.TopRight.Y})");

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error. " + e.Message);
                    }
                }
            }
        }
        public static void Save(Rectangle rectangle, string filePath)
        {
            string json = JsonConvert.SerializeObject(rectangle);
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Rectangle saved to {filePath}");
        }

        public static Rectangle Load(string filePath)
        {
            string json = File.ReadAllText(filePath);
            Rectangle rectangle = JsonConvert.DeserializeObject<Rectangle>(json);
            return rectangle;
        }
    }
}