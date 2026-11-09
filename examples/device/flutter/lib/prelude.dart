// The names a page's text supplies around a Dart fragment: your server's values, your app's own functions.
import 'dart:typed_data';

final serverNonce = Uint8List(32);
final challenge = Uint8List(32);
final ciphertext = Uint8List(1120);
final enc = Uint8List(32);
const ed25519PublicKey = '';
void show(Object? value) {}
void reload([Object? value]) {}
Future<void> sendToServer(Object? value) async {}
