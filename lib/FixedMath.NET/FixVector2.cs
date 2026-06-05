using System;
using Godot;

namespace FixMath.NET 
{
	[Serializable]
	public struct FixVector2 
	{
		public Fix64 X;
		public Fix64 Y;

		public FixVector2(Fix64 x, Fix64 y)
		{
			this.X = x;
			this.Y = y;
		}

		public static readonly FixVector2 Zero = new FixVector2();
		
		public static readonly FixVector2 One = new FixVector2(Fix64.One, Fix64.One);
		
		public static readonly FixVector2 Up = new FixVector2(Fix64.Zero, Fix64.One);
		public static readonly FixVector2 Down = new FixVector2(Fix64.Zero, Fix64.NegativeOne);
		public static readonly FixVector2 Right = new FixVector2(Fix64.One, Fix64.Zero);
		public static readonly FixVector2 Left = new FixVector2(Fix64.NegativeOne, Fix64.Zero);
		
		public static Fix64 Length(FixVector2 v)
		{
			return Fix64.Sqrt(v.X * v.X + v.Y * v.Y);
		}

		public static Fix64 Distance(FixVector2 vec0, FixVector2 vec1)
		{
			Fix64 dx = vec0.X - vec1.X;
			Fix64 dy = vec0.Y - vec1.Y;
			return Fix64.Sqrt(dx * dx + dy * dy);
		}

		public static Fix64 LengthSq(FixVector2 v)
		{
			return v.X * v.X + v.Y * v.Y;
		}

		public static Fix64 DistanceSq(FixVector2 vec0, FixVector2 vec1)
		{
			Fix64 dx = vec0.X - vec1.X;
			Fix64 dy = vec0.Y - vec1.Y;
			return dx * dx + dy * dy;
		}

		public static FixVector2 Normalize(FixVector2 v)
		{
			Fix64 len = Length(v);
			if (len == Fix64.Zero) return Zero;
			FixVector2 nor = new FixVector2(v.X / len, v.Y / len);
			//if (Fix64.Abs(nor.x) < Fix64.Epsilon) nor.x = Fix64.Zero;
			//if (Fix64.Abs(nor.y) < Fix64.Epsilon) nor.y = Fix64.Zero;
			return nor;
		}

		public static Fix64 Dot(FixVector2 a, FixVector2 b)
		{
			// a · b = ax * bx + ay * by
			return a.X * b.X + a.Y * b.Y;
		}

		public static Fix64 Cross(FixVector2 a, FixVector2 b)
		{
			// cz = ax * by − ay * bx
			return a.X * b.Y - a.Y * b.X;
		}

		public static FixVector2 GetNormal(FixVector2 a, FixVector2 b)
		{
			FixVector2 edge = b - a;
			FixVector2 axis = new FixVector2(-edge.Y, edge.X);
			return Normalize(axis);
		}

		public static FixVector2 GetInvertedNormal(FixVector2 a, FixVector2 b)
		{
			FixVector2 edge = b - a;
			FixVector2 axis = new FixVector2(edge.Y, -edge.X);
			return Normalize(axis);
		}


		public static FixVector2 Transform(FixVector2 v, FixVector2 refPosition, Fix64 refRotation)
		{
			FixVector2 r = Rotate(v, refRotation);

			Fix64 tx = r.X + refPosition.X;
			Fix64 ty = r.Y + refPosition.Y;

			return new FixVector2(tx, ty);
		}

		public static FixVector2 Project(FixVector2 u, FixVector2 v)
		{
			if (u == Zero) return Zero;
			if (v == Zero) return Zero;

			return Dot(v, u) / Dot(v, v) * v;
		}
		public static FixVector2 Reject(FixVector2 u, FixVector2 v)
		{
			return u - Project(u, v);
		}

		public static FixVector2 Rotate(FixVector2 v, Fix64 angle)
		{
			Fix64 rx = Fix64.Cos(angle) * v.X - Fix64.Sin(angle) * v.Y;
			Fix64 ry = Fix64.Sin(angle) * v.X + Fix64.Cos(angle) * v.Y;
			return new FixVector2(rx, ry);
		}

		public static Fix64 Angle(FixVector2 a, FixVector2 b)
		{
			//return Fix64.Atan2(b.y - a.y, b.x - a.x);
			Fix64 dot = Dot(a, b);
			Fix64 magA = Length(a);
			Fix64 magB = Length(b);
			if (magA * magB == Fix64.Zero) return Fix64.Zero;
			return Fix64.Acos(dot / (magA * magB));
		}

		public static Fix64 AngleDegrees(FixVector2 a, FixVector2 b)
		{
			return Angle(a, b) * Fix64.RadToDeg;
		}

		public static bool IsSameDirection(FixVector2 a, FixVector2 b)
		{
			return Dot(a, b) > Fix64.Zero;
		}

		public static bool Approximate(FixVector2 a, FixVector2 b)
		{
			return Fix64.Approximate(a.X, b.X) && Fix64.Approximate(a.Y, b.Y);
		}

		public static FixVector2 ClampMagnitude(FixVector2 vector, Fix64 magnitude)
		{
			if (Length(vector) > magnitude)
				return Normalize(vector) * magnitude;
			return vector;
		}
		
		public static FixVector2 MoveTowards(FixVector2 a, FixVector2 b, Fix64 v)
		{
			if (a == b) return b;
			FixVector2 direction = Normalize(b - a) * v;
			return a + direction;
		}


		public static FixVector2 operator +(FixVector2 a, FixVector2 b) {
			return new FixVector2(a.X + b.X, a.Y + b.Y);
		}

		public static FixVector2 operator -(FixVector2 a, FixVector2 b) {
			return new FixVector2(a.X - b.X, a.Y - b.Y);
		}

		public static FixVector2 operator -(FixVector2 a) {
			return new FixVector2(-a.X, -a.Y);
		}

		public static FixVector2 operator *(FixVector2 a, FixVector2 b) {
			return new FixVector2(a.X * b.X, a.Y * b.Y);
		}
		
		public static FixVector2 operator *(Fix64 a, FixVector2 b) {
			return new FixVector2(a * b.X, a * b.Y);
		}
		
		public static FixVector2 operator *(FixVector2 a, Fix64 b) {
			return new FixVector2(a.X * b, a.Y * b);
		}

		public static FixVector2 operator /(FixVector2 a, FixVector2 b) {
			return new FixVector2(a.X / b.X, a.Y / b.Y);
		}
		
		public static FixVector2 operator /(FixVector2 a, Fix64 b) {
			return new FixVector2(a.X / b, a.Y / b);
		}
		
		public static FixVector2 operator /(Fix64 a, FixVector2 b) {
			return new FixVector2(a / b.X, a / b.Y);
		}

		public static bool operator ==(FixVector2 a, FixVector2 b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator !=(FixVector2 a, FixVector2 b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

		public static bool operator >(FixVector2 a, FixVector2 b)
		{
			return a.X > b.X || a.Y > b.Y;
		}

		public static bool operator <(FixVector2 a, FixVector2 b)
		{
			return a.X < b.X || a.Y < b.Y;
		}

		public static bool operator >=(FixVector2 a, FixVector2 b)
		{
			return a.X >= b.X || a.Y >= b.Y;
		}

		public static bool operator <=(FixVector2 a, FixVector2 b)
		{
			return a.X <= b.X || a.Y <= b.Y;
		}

		public static explicit operator Vector2(FixVector2 value)
		{
			return new Vector2((float)value.X, (float)value.Y);
		}

		public static explicit operator FixVector2(Vector2 value) {
			return new FixVector2((Fix64)value.X, (Fix64)value.Y);
		}

		public static explicit operator Vector3(FixVector2 value) {
			return new Vector3((float)value.X, (float)value.Y, 0);
		}

		public static explicit operator FixVector2(Vector3 value) {
			return new FixVector2((Fix64)value.X, (Fix64)value.Y);
		}

		public override bool Equals(object obj)
		{
			return ((FixVector2)obj).X.RawValue == X.RawValue &&
				((FixVector2)obj).Y.RawValue == Y.RawValue;
		}

		public override int GetHashCode()
		{
			return X.RawValue.GetHashCode() + Y.RawValue.GetHashCode();
		}

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public bool Equals(FixVector2 other)
		{
			return X == other.X && Y == other.Y;
		}
	}
}
