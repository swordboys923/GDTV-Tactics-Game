


using System;

public struct GridPosition: IEquatable<GridPosition> {

    public int x;
    public int height;
    public int z;

    public GridPosition(int x, int z){
        this.x = x;
        this.height = 0;
        this.z = z;
    }

    public GridPosition(int x,int y, int z) {
        this.x = x;
        this.height = y;
        this.z = z;
    }

    public void UpdateGridPositionY(int y) {
        this.height = y;
    }

    public override bool Equals(object obj) {
        return obj is GridPosition position &&
               x == position.x &&
               height == position.height &&
               z == position.z;
    }

    public bool Equals(GridPosition other) {
        return this == other;
    }

    public override int GetHashCode() {
        return HashCode.Combine(x, height, z);
    }

    public override string ToString() {
        return $"x: {x}; z: {z}; height: {height}";
    }

    public static bool operator == (GridPosition a, GridPosition b) {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator != (GridPosition a, GridPosition b) {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b) {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }

    public static GridPosition operator -(GridPosition a, GridPosition b) {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }
}