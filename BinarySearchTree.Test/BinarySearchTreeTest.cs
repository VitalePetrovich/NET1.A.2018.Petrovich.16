using System.Collections.Generic;
using NUnit.Framework;
using BookLibrary;

namespace BinarySearchTree.Test
{
    using System.Text;

    class CustomComparerInt32 : Comparer<int>
    {
        public override int Compare(int x, int y)
        {
            return -x.CompareTo(y);
        }
    }

    class CustomComparerString : Comparer<string>
    {
        public override int Compare(string x, string y)
        {
            return -x.CompareTo(y);
        }
    }

    class CustomComparerBook : Comparer<Book>
    {
        public override int Compare(Book x, Book y)
        {
            return -x.CompareTo(y);
        }
    }

    struct Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class CustomComparerPoint : Comparer<Point>
    {
        public override int Compare(Point x, Point y)
        {
            return x.X.CompareTo(y.X);
        }
    }

    [TestFixture]
    public class BinarySearchTreeTest
    {
        [TestCase(new[] { 5, 12, 7, 6, 40, 35, 10, 1, 8, 4 }, ExpectedResult = "14567810123540")]
        public string BinarySearchTree_ValidIn_ValidOut(IEnumerable<int> array)
        {
            BinarySearchTree<int> tree = new BinarySearchTree<int>(array);
            StringBuilder actual = new StringBuilder();

            foreach (var item in tree.Inorder())
            {
                actual.Append(item);
            }

            return actual.ToString();
        }
    }
}
