/// Прямокутники, сторони || 0x(y). Можливо перемістити, змінити розмір. 
/// Побудова найменшого прямокутника з двох заданих і прямокутник-перетин
/// 1 метод для збереження об'єкту у JSON
/// 2 для відкриття та створення об'єкту.

using System;
using System.IO;
using Newtonsoft.Json;

namespace Lab2_3
{
    public class Rectangle
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; private set; }
        public double Y { get; private set; }

        public Rectangle(double width, double height, double x, double y)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        public void Move(double dx, double dy)
        {
            X += dx;
            Y += dy;
        }

        public void Resize(double newWidth, double newHeight)
        {
            Width = newWidth;
            Height = newHeight;
        }
        
        public static Rectangle Combine(Rectangle rect1, Rectangle rect2)
        {
            double minX = Math.Min(rect1.X, rect2.X);
            double minY = Math.Min(rect1.Y, rect2.Y);
            double maxX = Math.Max(rect1.X + rect1.Width, rect2.X + rect2.Width);
            double maxY = Math.Max(rect1.Y + rect1.Height, rect2.Y + rect2.Height);

            double newWidth = maxX - minX;
            double newHeight = maxY - minY;

            return new Rectangle(newWidth, newHeight, minX, minY);
        }

        public static Rectangle Intersection(Rectangle rect1, Rectangle rect2)
        {
            double left_x = Math.Max(rect1.X, rect2.X);
            double width = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width);
            double left_y = Math.Max(rect1.Y, rect2.Y);
            double height = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height);

            if (left_x < width && left_y < height)
            {
                double newWidth = width - left_x;
                double newHeight = height - left_y;
                return new Rectangle(newWidth, newHeight, left_x, left_y);
            }
            else
            {
                return new Rectangle(0, 0, 0, 0);
            }
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
                        Rectangle rect1 = new Rectangle(5, 3, 0, 0);
                        Rectangle rect2 = new Rectangle(4, 6, 2, 2);

                        rect1.Move(1, 1);

                        rect2.Resize(6, 8);

                        Rectangle combined = Rectangle.Combine(rect1, rect2);
                        Console.WriteLine("Combined Rectangle: Width = {0}, Height = {1}, X = {2}, Y = {3}",
                                          combined.Width, combined.Height, combined.X, combined.Y);
                        
                        Rectangle intersected = Rectangle.Intersection(rect1, rect2);
                        Console.WriteLine("Intersect Rectangle: Width = {0}, Height = {1}, X = {2}, Y = {3}",
                                          intersected.Width, intersected.Height, intersected.X, intersected.Y);
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
                        Rectangle rectangle = new Rectangle(10, 5, 2, 3);

                        Save(rectangle, "D:\\Projects_VS\\Lab2_3\\rectangle.json");

                        Rectangle restoredRectangle = Load("D:\\Projects_VS\\Lab2_3\\rectangle.json");

                        Console.WriteLine("Restored Rectangle:");
                        Console.WriteLine($"Width: {restoredRectangle.Width}, Height: {restoredRectangle.Height}, " +
                            $"X: {restoredRectangle.X}, Y: {restoredRectangle.Y}");
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