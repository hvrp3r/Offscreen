import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:offscreen_mobile/main.dart';

void main() {
  testWidgets('shows the welcome message in English', (tester) async {
    await tester.pumpWidget(const OffscreenMobileApp());

    expect(find.text('Your media center is ready.'), findsOneWidget);
  });

  testWidgets('shows the welcome message in French', (tester) async {
    tester.platformDispatcher.localesTestValue = const [Locale('fr')];
    addTearDown(tester.platformDispatcher.clearLocalesTestValue);

    await tester.pumpWidget(const OffscreenMobileApp());
    await tester.pumpAndSettle();

    expect(find.text('Votre media center est prêt.'), findsOneWidget);
  });
}
