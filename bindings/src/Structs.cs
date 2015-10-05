using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;

namespace Urho {
	[StructLayout (LayoutKind.Sequential)]
	public struct Ray {
		public Vector3 Origin;
		public Vector3 Direction;

		public Ray(Vector3 origin, Vector3 direction)
		{
			Origin = origin;
			Direction = Vector3.Normalize(direction);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsRaycastResult {
		public Vector3 Position;
		public Vector3 Normal;
		public float Distance;

		IntPtr bodyPtr;
		public RigidBody Body => Runtime.LookupObject<RigidBody>(bodyPtr);
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct IntRect {
		public int Left, Top, Right, Bottom;
		public IntRect (int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct Rect {
		public Vector2 Min, Max;

		byte defined; //bool is not blittable.
		public bool Defined { get { return defined != 0; } set { defined = (byte)(value ? 1 : 0); } }

		public Rect (int left, int top, int right, int bottom)
		{
			Min = new Vector2 (left, top);
			Max = new Vector2 (right, bottom);
			defined = 1;
		}
		
		public Rect (Vector2 min, Vector2 max)
		{
			Min = min;
			Max = max;
			defined = 1;
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct ResourceRef {
		public StringHash Type;
		public UrhoString Name;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct HashIteratorBase {
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct Iterator {
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct ResourceRefList {
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct BoundingBox {
		public Vector3 Min, Max;

		byte defined; //bool is not blittable.
		public bool Defined { get { return defined != 0; } set { defined = (byte)(value ? 1 : 0); } }

		public BoundingBox (float min, float max)
		{
			Min = new Vector3 (min, min, min);
			Max = new Vector3 (max, max, max);
			defined = 1;
		}

		public BoundingBox (Vector3 min, Vector3 max)
		{
			Min = min;
			Max = max;
			defined = 1;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AnimationTriggerPoint {
		public float Time;
		public IntPtr Variant;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct Matrix3x4 {
		public float m00;
		public float m01;
		public float m02;
		public float m03;
		public float m10;
		public float m11;
		public float m12;
		public float m13;
		public float m20;
		public float m21;
		public float m22;
		public float m23;
		
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct Color {
		public float R, G, B, A;

		public Color (float r = 1f, float g = 1f, float b = 1f, float a = 1f)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public Color (Color source)
		{
			R = source.R;
			G = source.G;
			B = source.B;
			A = source.A;
		}

		public Color (Color source, float alpha)
		{
			R = source.R;
			G = source.G;
			B = source.B;
			A = alpha;
		}

		public static Color White = new Color (1, 1, 1);
		public static Color Gray = new Color (0.5f, 0.5f, 0.5f);
		public static Color Black = new Color (0.0f, 0.0f, 0.0f);
		public static Color Red = new Color (1.0f, 0.0f, 0.0f);
		public static Color Green = new Color (0.0f, 1.0f, 0.0f);
		public static Color Blue = new Color (0.0f, 0.0f, 1.0f);
		public static Color Cyan = new Color (0.0f, 1.0f, 1.0f);
		public static Color Magenta = new Color (1.0f, 0.0f, 1.0f);
		public static Color Yellow = new Color (1.0f, 1.0f, 0.0f);
		public static Color Transparent = new Color (0.0f, 0.0f, 0.0f, 0.0f);
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct Frustum {
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct Variant {
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct XPathResultSet {
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct WeakPtr {
		internal IntPtr ptr;
		internal IntPtr refCountPtr;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct TouchState {
		public int TouchID;
		public IntVector2 Position, LastPosition, Delta;
		public float Pressure;
		private WeakPtr _TouchedElement;

		[DllImport ("mono-urho", CallingConvention=CallingConvention.Cdecl)]
		extern static IntPtr TouchState_GetTouchedElement (ref TouchState state);
		
		public UIElement TouchedElement ()
		{
			return Runtime.LookupObject<UIElement>(_TouchedElement.ptr);

			var x = TouchState_GetTouchedElement (ref this);
			if (x == IntPtr.Zero)
				return null;
			
			return Runtime.LookupObject<UIElement> (x);
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct ColorFrame {
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct JoystickState {
		public IntPtr JoystickPtr;
		public IntPtr JoystickIdPtr;
		public IntPtr ControllerPtr;
		public IntPtr ScreenJoystickPtr;
		public UIElement ScreenJoystick => Runtime.LookupObject<UIElement>(ScreenJoystickPtr);
        public UrhoString Name;
		public VectorBase Buttons;
		public VectorBase ButtonPress;
		public VectorBase Axes;
		public VectorBase Hats;

		public bool GetButtonDown(int position) => Axes.ToArray<byte>()[position] != 0;
		public bool GetButtonPress(int position) => Axes.ToArray<byte>()[position] != 0;
		public float GetAxisPosition(int position) => Axes.ToArray<float>()[position];
		public int GetHatPosition(int position) => Axes.ToArray<int>()[position];
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VectorBase {
		public uint Size;
		public uint Capacity;
		public IntPtr Buffer;

		public T[] ToArray<T>() where T : struct 
		{
			T[] result = new T[Size];
			var typeSize = Marshal.SizeOf(typeof (T));
			for (int i = 0; i < Size; i++)
			{
				var ptr = Marshal.ReadIntPtr(Buffer, typeSize*i);
				result[i] = (T)Marshal.PtrToStructure(ptr, typeof (T));
			}
			return result;
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct TextureFrame {
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct LightBatchQueue {
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct Bone {
		public UrhoString Name;
		public int NameHash;
		public uint ParentIndex;
		public Vector3 InitialPosition;
		public Quaternion InitialRotation;
		public Vector3 InitialScale;
		public Matrix3x4 OffsetMatrix;

		byte animated; //bool is not blittable.
		public bool Animated { get { return animated != 0; } set { animated = (byte)(value ? 1 : 0); } }

		public int CollisionMask;
		public float Radius;
		public BoundingBox BoundingBox;
		private WeakPtr Node;
	}

	public unsafe class BoneWrapper
	{
		readonly object objHolder;
		readonly Bone* b;

		public BoneWrapper(object objHolder, Bone* bone)
		{
			this.objHolder = objHolder;
			this.b = bone;
		}

		public UrhoString Name { get { return b->Name; } set { b->Name = value; } }
		public int NameHash { get { return b->NameHash; } set { b->NameHash = value; } }
		public uint ParentIndex { get { return b->ParentIndex; } set { b->ParentIndex = value; } }
		public Vector3 InitialPosition { get { return b->InitialPosition; } set { b->InitialPosition = value; } }
		public Quaternion InitialRotation { get { return b->InitialRotation; } set { b->InitialRotation = value; } }
		public Vector3 InitialScale { get { return b->InitialScale; } set { b->InitialScale = value; } }
		public Matrix3x4 OffsetMatrix { get { return b->OffsetMatrix; } set { b->OffsetMatrix = value; } }
		public bool Animated { get { return b->Animated; } set { b->Animated = value; } }
		public int CollisionMask { get { return b->CollisionMask; } set { b->CollisionMask = value; } }
		public float Radius { get { return b->Radius; } set { b->Radius = value; } }
		public BoundingBox BoundingBox { get { return b->BoundingBox; } set { b->BoundingBox = value; } }
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout(LayoutKind.Sequential)]
	public struct RayQueryResult {
		public Vector3 Position;
		public Vector3 Normal;
		public Vector2 TextureUV;
		public float Distance;

		IntPtr drawablePtr;
		public Drawable Drawable => Runtime.LookupObject<Drawable>(drawablePtr);

		IntPtr nodePtr;
		public Node Node => Runtime.LookupObject<Node>(nodePtr);

		public uint SubObject;
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout(LayoutKind.Sequential)]
	public struct TileMapInfo2D {
		public Orientation2D Orientation;
		public int Width;
		public int Height;
		public float TileWidth;
		public float TileHeight;

		//calculated properties:
		public float MapWidth => Width * TileWidth;
		public float MapHeight
		{
			get
			{
				if (Orientation == Orientation2D.O_STAGGERED)
					return (Height + 1) * 0.5f * TileHeight;
				return Height * TileHeight;
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct dtQueryFilter {
		//public float[] AreaCost;     // Cost per area type. (Used by default implementation.)
		//public ushort IncludeFlags;  // Flags for polygons that can be visited. (Used by default implementation.)
		//public ushort ExcludeFlags;  // Flags for polygons that should not be visted. (Used by default implementation.)
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct ReplicationState {
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct NodeReplicationState {
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct RenderPathCommand {
	}
	
	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct GPUObject {
	}
	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct GraphicsImpl {
	}
	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct FontGlyph {
	}
	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct RandomAccessIterator {
	}
	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct ModelMorph {
	}
	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct Octant {
	}
	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct CompressedLevel {
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct Billboard {
		public Vector3 Position;
		public Vector2 Size;
		public Rect Uv;
		public Color Color;
		public float Rotation;

		byte enabled; //bool is not blittable.
		public bool Enabled { get { return enabled != 0; } set { enabled = (byte)(value ? 1 : 0); } }

		public float SortDistance;
	}
	
	public unsafe class BillboardWrapper
	{
		readonly object bbHolder;
		readonly Billboard* bb;

		public BillboardWrapper(object bbHolder, Billboard* bb)
		{
			this.bbHolder = bbHolder;
			this.bb = bb;
		}

		public Vector3 Position { get { return bb->Position; } set { bb->Position = value; } }
		public Vector2 Size { get { return bb->Size; } set { bb->Size = value; } }
		public Rect Uv { get { return bb->Uv; } set { bb->Uv = value; } }
		public Color Color { get { return bb->Color; } set { bb->Color = value; } }
		public float Rotation { get { return bb->Rotation; } set { bb->Rotation = value; } }
		public bool Enabled { get { return bb->Enabled; } set { bb->Enabled = value; } }
		public float SortDistance { get { return bb->SortDistance; } set { bb->SortDistance = value; } }
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct AnimationTrack {
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct CustomGeometryVertex {
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct NetworkState {
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct ComponentReplicationState {
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct ShaderParameter {
	}

	// DEBATABLE: maybe we should let the binder handle it?
	[StructLayout (LayoutKind.Sequential)]
	public struct UrhoString
	{
		public uint Length;
		public uint Capacity;
		public IntPtr Buffer;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct PackageEntry {
	}


	[StructLayout(LayoutKind.Sequential)]
	public struct CrowdObstacleAvoidanceParams {
		public float VelBias;
		public float WeightDesVel;
		public float WeightCurVel;
		public float WeightSide;
		public float WeightToi;
		public float HorizTime;
		public byte GridSize;
		public byte AdaptiveDivs;
		public byte AdaptiveRings;
		public byte AdaptiveDepth;
	};

	[StructLayout (LayoutKind.Sequential)]
	public struct BiasParameters {
		public float ConstantBias;
		public float SlopeScaleBias;

		public BiasParameters (float constantBias, float slopeScaleBias)
		{
			ConstantBias = constantBias;
			SlopeScaleBias = slopeScaleBias;
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct CascadeParameters {
		public float Split1, Split2, Split3, Split4;
		public float FadeStart;
		public float BiasAutoAdjust;

		public CascadeParameters (float split1, float split2, float split3, float split4, float fadeStart, float biasAutoAdjust = 1f)
		{
			Split1 = split1;
			Split2 = split2;
			Split3 = split3;
			Split4 = split4;
			FadeStart = fadeStart;
			BiasAutoAdjust = biasAutoAdjust;
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct FocusParameters {
		public bool Focus;
		public bool NonUniform;
		public bool AutoSize;
		public float Quantize;
		public float MinView;
	}
}


namespace System {
	//
	// Hacks until I get Sharpie to not mess with my types
	//
	[StructLayout (LayoutKind.Sequential)]
	public struct nuint {
		UIntPtr x;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct nint {
		IntPtr x;
	}

}
