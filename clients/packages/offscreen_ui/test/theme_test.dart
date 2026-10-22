import 'package:flutter_test/flutter_test.dart';
import 'package:flutter/material.dart';
import 'package:offscreen_ui/offscreen_ui.dart';

void main() {
  test('dark theme uses a dark color scheme', () {
    expect(offscreenDarkTheme().colorScheme.brightness, Brightness.dark);
  });
}
