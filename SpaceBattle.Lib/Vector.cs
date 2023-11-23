public class Vector
{
    private int[] _array { get; set; }
    private int _dimension { get; }
    public Vector(int[] coor)
    {
        if (coor.Length > 0)
        {
            _array = coor;
            _dimension = coor.Length;
        }
        else
        {
            throw new System.ArgumentException();
        }
    }

    public static Vector operator +(Vector array_1, Vector array_2)
    {
        if (array_1._dimension == array_2._dimension)
        {
            var array_out = new Vector(new int[array_1._dimension]);
            for (var i = 0; i < array_1._dimension; i++)
            {
                array_out._array[i] = array_1._array[i] + array_2._array[i];
            }

            return array_out;
        }
        else
        {
            throw new System.ArgumentException();
        }
    }

    public static Vector operator -(Vector array_1, Vector array_2)
    {
        if (array_1._dimension == array_2._dimension)
        {
            var array_out = new Vector(new int[array_1._dimension]);
            for (var i = 0; i < array_1._dimension; i++)
            {
                array_out._array[i] = array_1._array[i] - array_2._array[i];
            }

            return array_out;
        }
        else
        {
            throw new System.ArgumentException();
        }
    }

    public static bool operator ==(Vector array_1, Vector array_2)
    {
        if (array_1._dimension == array_2._dimension)
        {
            var comparison_result = true;
            for (var i = 0; i < array_1._dimension; i++)
            {
                if (array_1._array[i] != array_2._array[i])
                {
                    comparison_result = false;
                    break;

                }
            }

            return comparison_result;
        }
        else
        {
            throw new System.ArgumentException();
        }
    }

    public static bool operator !=(Vector array_1, Vector array_2)
    {
        if (array_1._dimension == array_2._dimension)
        {
            var comparison_result = true;
            for (var i = 0; i < array_1._dimension; i++)
            {
                if (array_1._array[i] == array_2._array[i])
                {
                    comparison_result = false;
                    break;

                }
            }

            return comparison_result;
        }
        else
        {
            throw new ArgumentException();
        }
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        
        return obj is Vector vector && this == vector;
    }
}
