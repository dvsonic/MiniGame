// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: common.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Message {

  /// <summary>Holder for reflection information generated from common.proto</summary>
  public static partial class CommonReflection {

    #region Descriptor
    /// <summary>File descriptor for common.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CommonReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cgxjb21tb24ucHJvdG8SB01lc3NhZ2UipgEKCk5ldE1lc3NhZ2UaLAoJRVBy",
            "b3RvY29sIh8KBVByb3RvEg4KClByb3RvX05PTkUQABIGCgJBSRABIj4KD1By",
            "b3RvY29sVmVyc2lvbhIICgROT05FEAASDgoHVkVSU0lPThDBlIkKEhEKCk1J",
            "TlZFUlNJT04Q6Y+JCiIqCglFcnJvckNvZGUSBgoCT0sQABIVChFSRVFfUEFS",
            "QU1fSU5WQUxJRBABIi0KClZlY3RvcjNJbnQSCQoBeBgBIAEoBRIJCgF5GAIg",
            "ASgFEgkKAXoYAyABKAViBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Message.NetMessage), global::Message.NetMessage.Parser, null, null, new[]{ typeof(global::Message.NetMessage.Types.ProtocolVersion), typeof(global::Message.NetMessage.Types.ErrorCode) }, new pbr::GeneratedClrTypeInfo[] { new pbr::GeneratedClrTypeInfo(typeof(global::Message.NetMessage.Types.EProtocol), global::Message.NetMessage.Types.EProtocol.Parser, null, null, new[]{ typeof(global::Message.NetMessage.Types.EProtocol.Types.Proto) }, null)}),
            new pbr::GeneratedClrTypeInfo(typeof(global::Message.Vector3Int), global::Message.Vector3Int.Parser, new[]{ "X", "Y", "Z" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class NetMessage : pb::IMessage<NetMessage> {
    private static readonly pb::MessageParser<NetMessage> _parser = new pb::MessageParser<NetMessage>(() => new NetMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<NetMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Message.CommonReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public NetMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public NetMessage(NetMessage other) : this() {
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public NetMessage Clone() {
      return new NetMessage(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as NetMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(NetMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(NetMessage other) {
      if (other == null) {
        return;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the NetMessage message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public enum ProtocolVersion {
        [pbr::OriginalName("NONE")] None = 0,
        /// <summary>
        /// 当前协议版本号
        /// </summary>
        [pbr::OriginalName("VERSION")] Version = 21121601,
        /// <summary>
        /// 最小允许协议版本号
        /// </summary>
        [pbr::OriginalName("MINVERSION")] Minversion = 21121001,
      }

      public enum ErrorCode {
        [pbr::OriginalName("OK")] Ok = 0,
        [pbr::OriginalName("REQ_PARAM_INVALID")] ReqParamInvalid = 1,
      }

      public sealed partial class EProtocol : pb::IMessage<EProtocol> {
        private static readonly pb::MessageParser<EProtocol> _parser = new pb::MessageParser<EProtocol>(() => new EProtocol());
        private pb::UnknownFieldSet _unknownFields;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pb::MessageParser<EProtocol> Parser { get { return _parser; } }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pbr::MessageDescriptor Descriptor {
          get { return global::Message.NetMessage.Descriptor.NestedTypes[0]; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        pbr::MessageDescriptor pb::IMessage.Descriptor {
          get { return Descriptor; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public EProtocol() {
          OnConstruction();
        }

        partial void OnConstruction();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public EProtocol(EProtocol other) : this() {
          _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public EProtocol Clone() {
          return new EProtocol(this);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override bool Equals(object other) {
          return Equals(other as EProtocol);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public bool Equals(EProtocol other) {
          if (ReferenceEquals(other, null)) {
            return false;
          }
          if (ReferenceEquals(other, this)) {
            return true;
          }
          return Equals(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override int GetHashCode() {
          int hash = 1;
          if (_unknownFields != null) {
            hash ^= _unknownFields.GetHashCode();
          }
          return hash;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override string ToString() {
          return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void WriteTo(pb::CodedOutputStream output) {
          if (_unknownFields != null) {
            _unknownFields.WriteTo(output);
          }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public int CalculateSize() {
          int size = 0;
          if (_unknownFields != null) {
            size += _unknownFields.CalculateSize();
          }
          return size;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(EProtocol other) {
          if (other == null) {
            return;
          }
          _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(pb::CodedInputStream input) {
          uint tag;
          while ((tag = input.ReadTag()) != 0) {
            switch(tag) {
              default:
                _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                break;
            }
          }
        }

        #region Nested types
        /// <summary>Container for nested types declared in the EProtocol message type.</summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static partial class Types {
          public enum Proto {
            [pbr::OriginalName("Proto_NONE")] None = 0,
            [pbr::OriginalName("AI")] Ai = 1,
          }

        }
        #endregion

      }

    }
    #endregion

  }

  public sealed partial class Vector3Int : pb::IMessage<Vector3Int> {
    private static readonly pb::MessageParser<Vector3Int> _parser = new pb::MessageParser<Vector3Int>(() => new Vector3Int());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Vector3Int> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Message.CommonReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Vector3Int() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Vector3Int(Vector3Int other) : this() {
      x_ = other.x_;
      y_ = other.y_;
      z_ = other.z_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Vector3Int Clone() {
      return new Vector3Int(this);
    }

    /// <summary>Field number for the "x" field.</summary>
    public const int XFieldNumber = 1;
    private int x_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "y" field.</summary>
    public const int YFieldNumber = 2;
    private int y_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    /// <summary>Field number for the "z" field.</summary>
    public const int ZFieldNumber = 3;
    private int z_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Z {
      get { return z_; }
      set {
        z_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Vector3Int);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Vector3Int other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (X != other.X) return false;
      if (Y != other.Y) return false;
      if (Z != other.Z) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (X != 0) hash ^= X.GetHashCode();
      if (Y != 0) hash ^= Y.GetHashCode();
      if (Z != 0) hash ^= Z.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (X != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(X);
      }
      if (Y != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Y);
      }
      if (Z != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Z);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (X != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(X);
      }
      if (Y != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Y);
      }
      if (Z != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Z);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Vector3Int other) {
      if (other == null) {
        return;
      }
      if (other.X != 0) {
        X = other.X;
      }
      if (other.Y != 0) {
        Y = other.Y;
      }
      if (other.Z != 0) {
        Z = other.Z;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            X = input.ReadInt32();
            break;
          }
          case 16: {
            Y = input.ReadInt32();
            break;
          }
          case 24: {
            Z = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
