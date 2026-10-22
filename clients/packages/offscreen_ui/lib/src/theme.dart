import 'package:flutter/material.dart';

const offscreenPurple = Color(0xFF7C4DFF);

ThemeData offscreenDarkTheme() => ThemeData(
  colorScheme: ColorScheme.fromSeed(
    seedColor: offscreenPurple,
    brightness: Brightness.dark,
  ),
);
