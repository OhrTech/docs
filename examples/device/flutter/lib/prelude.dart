// The names a page's text supplies around a Dart fragment: your server's values, your app's own functions.
import 'dart:typed_data';

import 'package:flutter/material.dart';

final serverNonce = Uint8List(32);
final challenge = Uint8List(32);
final ciphertext = Uint8List(1120);
final enc = Uint8List(32);
const ed25519PublicKey = '';
const mlDsa65PublicKey = '';
final theirPublicKey = Uint8List(1184);
const staffPassword = '';
void show(Object? value) {}
void reload([Object? value]) {}
Future<void> sendToServer(Object? value) async {}
void applyTheme({required bool dark}) {}

/// Your own server's API, as the attestation page calls it.
class MyServer {
  Future<String> newChallenge() async => '';
  Future<void> register(String json) async {}
}

final myServer = MyServer();

/// The app's root widget, as `runApp(const MyApp())` in main() names it.
class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) => const MaterialApp(home: Placeholder());
}
