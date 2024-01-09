using System;

namespace com.calicraft.vrjester.utils.tools
{
	using Codec = com.mojang.serialization.Codec;
	using Util = net.minecraft.Util;
	using NotNull = org.jetbrains.annotations.NotNull;
	using Vector3f = org.joml.Vector3f;

	public class Vec3 : net.minecraft.world.phys.Vec3
	{
		// This class is to de-obfuscate the Minecraft Vec3 class
		public static readonly Codec<Vec3> CODEC = Codec.DOUBLE.listOf().comapFlatMap((p_231079_) =>
		{
		return Util.fixedSize(p_231079_, 3).map((p_231081_) =>
		{
			return new Vec3(p_231081_.get(0), p_231081_.get(1), p_231081_.get(2));
		});
		}, (p_231083_) =>
		{
		return List.of(p_231083_.x(), p_231083_.y(), p_231083_.z());
	});
		public static readonly Vec3 ZERO = new Vec3(0.0D, 0.0D, 0.0D);
		public Vec3(double x, double y, double z) : base(x, y, z)
		{
		}

		public Vec3(Vector3f vector3f) : base(vector3f)
		{
		}

		public Vec3(net.minecraft.world.phys.Vec3 vector3) : base(vector3.x, vector3.y, vector3.z)
		{
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public @NotNull Vec3 normalize()
		public virtual Vec3 normalize()
		{
			double d0 = Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
			return d0 < 1.0E-4D ? (Vec3) ZERO : (Vec3) new Vec3(this.x / d0, this.y / d0, this.z / d0);
		}

		public virtual double dot(Vec3 vec3)
		{
			return this.x * vec3.x + this.y * vec3.y + this.z * vec3.z;
		}

		public virtual Vec3 cross(Vec3 vec3)
		{
			return new Vec3(this.y * vec3.z - this.z * vec3.y, this.z * vec3.x - this.x * vec3.z, this.x * vec3.y - this.y * vec3.x);
		}

		public virtual Vec3 subtract(Vec3 vec3)
		{
			return this.subtract(vec3.x, vec3.y, vec3.z);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public @NotNull Vec3 subtract(double x, double y, double z)
		public virtual Vec3 subtract(double x, double y, double z)
		{
			return this.add(-x, -y, -z);
		}

		public virtual Vec3 add(Vec3 vec3)
		{
			return this.add(vec3.x, vec3.y, vec3.z);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public @NotNull Vec3 add(double x, double y, double z)
		public virtual Vec3 add(double x, double y, double z)
		{
			return new Vec3(this.x + x, this.y + y, this.z + z);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public @NotNull Vec3 scale(double scalar)
		public virtual Vec3 scale(double scalar)
		{
			return this.multiply(scalar, scalar, scalar);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public @NotNull Vec3 reverse()
		public virtual Vec3 reverse()
		{
			return this.scale(-1.0D);
		}

		public virtual Vec3 multiply(Vec3 vec3)
		{
			return this.multiply(vec3.x, vec3.y, vec3.z);
		}

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public @NotNull Vec3 multiply(double x, double y, double z)
		public virtual Vec3 multiply(double x, double y, double z)
		{
			return new Vec3(this.x * x, this.y * y, this.z * z);
		}

		public virtual double length()
		{
			return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
		}

		public virtual double lengthSqr()
		{
			return this.x * this.x + this.y * this.y + this.z * this.z;
		}

		public virtual double horizontalDistance()
		{
			return Math.Sqrt(this.x * this.x + this.z * this.z);
		}

		public virtual double horizontalDistanceSqr()
		{
			return this.x * this.x + this.z * this.z;
		}

		public override bool Equals(object @object)
		{
			if (this == @object)
			{
				return true;
			}
			else if (!(@object is Vec3 vec3))
			{
				return false;
			}
			else
			{
				if (vec3.x.CompareTo(this.x) != 0)
				{
					return false;
				}
				else if (vec3.y.CompareTo(this.y) != 0)
				{
					return false;
				}
				else
				{
					return vec3.z.CompareTo(this.z) == 0;
				}
			}
		}
	}

}
