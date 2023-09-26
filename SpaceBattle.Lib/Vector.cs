public class Vector{
    public int[] array_square {get; set;}
    public Vector (int[] in_position){ array_square = in_position ;}
    public static Vector Plus(Vector array_1, Vector  array_2)
    {  
        
        var array_out = new Vector(new int[2]);
        array_out.array_square[0] = array_1.array_square[0] + array_2.array_square[0];
        array_out.array_square[1] = array_1.array_square[1] + array_2.array_square[1];
        return array_out;
    }
}