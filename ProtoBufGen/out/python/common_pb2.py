# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: common.proto

import sys
_b=sys.version_info[0]<3 and (lambda x:x) or (lambda x:x.encode('latin1'))
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor.FileDescriptor(
  name='common.proto',
  package='Message',
  syntax='proto3',
  serialized_options=None,
  serialized_pb=_b('\n\x0c\x63ommon.proto\x12\x07Message\"\xa6\x01\n\nNetMessage\x1a,\n\tEProtocol\"\x1f\n\x05Proto\x12\x0e\n\nProto_NONE\x10\x00\x12\x06\n\x02\x41I\x10\x01\">\n\x0fProtocolVersion\x12\x08\n\x04NONE\x10\x00\x12\x0e\n\x07VERSION\x10\xc1\x94\x89\n\x12\x11\n\nMINVERSION\x10\xe9\x8f\x89\n\"*\n\tErrorCode\x12\x06\n\x02OK\x10\x00\x12\x15\n\x11REQ_PARAM_INVALID\x10\x01\"-\n\nVector3Int\x12\t\n\x01x\x18\x01 \x01(\x05\x12\t\n\x01y\x18\x02 \x01(\x05\x12\t\n\x01z\x18\x03 \x01(\x05\x62\x06proto3')
)



_NETMESSAGE_EPROTOCOL_PROTO = _descriptor.EnumDescriptor(
  name='Proto',
  full_name='Message.NetMessage.EProtocol.Proto',
  filename=None,
  file=DESCRIPTOR,
  values=[
    _descriptor.EnumValueDescriptor(
      name='Proto_NONE', index=0, number=0,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='AI', index=1, number=1,
      serialized_options=None,
      type=None),
  ],
  containing_type=None,
  serialized_options=None,
  serialized_start=53,
  serialized_end=84,
)
_sym_db.RegisterEnumDescriptor(_NETMESSAGE_EPROTOCOL_PROTO)

_NETMESSAGE_PROTOCOLVERSION = _descriptor.EnumDescriptor(
  name='ProtocolVersion',
  full_name='Message.NetMessage.ProtocolVersion',
  filename=None,
  file=DESCRIPTOR,
  values=[
    _descriptor.EnumValueDescriptor(
      name='NONE', index=0, number=0,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='VERSION', index=1, number=21121601,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='MINVERSION', index=2, number=21121001,
      serialized_options=None,
      type=None),
  ],
  containing_type=None,
  serialized_options=None,
  serialized_start=86,
  serialized_end=148,
)
_sym_db.RegisterEnumDescriptor(_NETMESSAGE_PROTOCOLVERSION)

_NETMESSAGE_ERRORCODE = _descriptor.EnumDescriptor(
  name='ErrorCode',
  full_name='Message.NetMessage.ErrorCode',
  filename=None,
  file=DESCRIPTOR,
  values=[
    _descriptor.EnumValueDescriptor(
      name='OK', index=0, number=0,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='REQ_PARAM_INVALID', index=1, number=1,
      serialized_options=None,
      type=None),
  ],
  containing_type=None,
  serialized_options=None,
  serialized_start=150,
  serialized_end=192,
)
_sym_db.RegisterEnumDescriptor(_NETMESSAGE_ERRORCODE)


_NETMESSAGE_EPROTOCOL = _descriptor.Descriptor(
  name='EProtocol',
  full_name='Message.NetMessage.EProtocol',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
    _NETMESSAGE_EPROTOCOL_PROTO,
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=40,
  serialized_end=84,
)

_NETMESSAGE = _descriptor.Descriptor(
  name='NetMessage',
  full_name='Message.NetMessage',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
  ],
  extensions=[
  ],
  nested_types=[_NETMESSAGE_EPROTOCOL, ],
  enum_types=[
    _NETMESSAGE_PROTOCOLVERSION,
    _NETMESSAGE_ERRORCODE,
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=26,
  serialized_end=192,
)


_VECTOR3INT = _descriptor.Descriptor(
  name='Vector3Int',
  full_name='Message.Vector3Int',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='x', full_name='Message.Vector3Int.x', index=0,
      number=1, type=5, cpp_type=1, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='y', full_name='Message.Vector3Int.y', index=1,
      number=2, type=5, cpp_type=1, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='z', full_name='Message.Vector3Int.z', index=2,
      number=3, type=5, cpp_type=1, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=194,
  serialized_end=239,
)

_NETMESSAGE_EPROTOCOL.containing_type = _NETMESSAGE
_NETMESSAGE_EPROTOCOL_PROTO.containing_type = _NETMESSAGE_EPROTOCOL
_NETMESSAGE_PROTOCOLVERSION.containing_type = _NETMESSAGE
_NETMESSAGE_ERRORCODE.containing_type = _NETMESSAGE
DESCRIPTOR.message_types_by_name['NetMessage'] = _NETMESSAGE
DESCRIPTOR.message_types_by_name['Vector3Int'] = _VECTOR3INT
_sym_db.RegisterFileDescriptor(DESCRIPTOR)

NetMessage = _reflection.GeneratedProtocolMessageType('NetMessage', (_message.Message,), {

  'EProtocol' : _reflection.GeneratedProtocolMessageType('EProtocol', (_message.Message,), {
    'DESCRIPTOR' : _NETMESSAGE_EPROTOCOL,
    '__module__' : 'common_pb2'
    # @@protoc_insertion_point(class_scope:Message.NetMessage.EProtocol)
    })
  ,
  'DESCRIPTOR' : _NETMESSAGE,
  '__module__' : 'common_pb2'
  # @@protoc_insertion_point(class_scope:Message.NetMessage)
  })
_sym_db.RegisterMessage(NetMessage)
_sym_db.RegisterMessage(NetMessage.EProtocol)

Vector3Int = _reflection.GeneratedProtocolMessageType('Vector3Int', (_message.Message,), {
  'DESCRIPTOR' : _VECTOR3INT,
  '__module__' : 'common_pb2'
  # @@protoc_insertion_point(class_scope:Message.Vector3Int)
  })
_sym_db.RegisterMessage(Vector3Int)


# @@protoc_insertion_point(module_scope)