using UnityEngine;

public static class Move {
    private static Vector3 left= new Vector3(-1,0,0);
    private static Vector3 right = new Vector3(1, 0, 0);
    private static Vector3 up = new Vector3(0, 1, 0);
    private static Vector3 down = new Vector3(0, -1, 0);
    public static Vector3 moveLeft()
    {
        return left;
    }
    public static Vector3 moveRight()
    {
        return right;
    }
    public static Vector3 moveUp()
    {
        return up;
    }
    public static Vector3 moveDown()
    {
        return down;
    }
}
