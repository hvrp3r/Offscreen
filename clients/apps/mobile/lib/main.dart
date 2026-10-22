import 'package:flutter/material.dart';
import 'package:offscreen_l10n/offscreen_l10n.dart';
import 'package:offscreen_ui/offscreen_ui.dart';

void main() {
  runApp(const OffscreenMobileApp());
}

class OffscreenMobileApp extends StatelessWidget {
  const OffscreenMobileApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      onGenerateTitle: (context) => OffscreenLocalizations.of(context).appTitle,
      theme: offscreenDarkTheme(),
      localizationsDelegates: OffscreenLocalizations.localizationsDelegates,
      supportedLocales: OffscreenLocalizations.supportedLocales,
      home: Builder(
        builder: (context) => Scaffold(
          body: Center(child: Text(OffscreenLocalizations.of(context).welcome)),
        ),
      ),
    );
  }
}
