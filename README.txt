使い方

プラットフォームごとに変更したいUIがある場合に使用できます。

１．シーン内の任意のオブジェクトに「PlatformOverriderGroup」をアタッチします。
２．座標や大きさを変えたいオブジェクトに「PlatformOverrider」をアタッチします。
　　変更したいオブジェクトごとに1つ必要です。
　　１で「PlatformOverriderGroup」を付与した子要素のみ有効です。
３．任意のプラットフォームを選択し、デザインの調整を行ってください。
　　この時RectTransformではなく、PlatformOverriderにて調整を行ってください。
　　また、WindowsとMac、PS4、PS5は共通で、スマホの横だけ別デザインがいい
　　などという際はDefaultを設定し、それぞれのプラットフォームでUseDefaultにチェックを入れてください。

