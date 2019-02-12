# Unity Architecture Template

## フォルダ構成

### main project

```
Assets/Scripts/Main
    - Addapter
        - Infrastructure
        - UI
    - DI
    - Domain
        - [ContextName]
            - IHogeRepository.cs
            - IHoge.cs
            - ValueObject.cs
        - DomainError.cs
        - DomainResult.cs
        - IEntity.cs
        - IValueObject.cs
    - UseCase
        - [ContextName]
        - ApplicationError.cs
        - ApplicationResult.cs
        - IUseCase.cs
```

* Assets/Scripts/Main
    * メインプロジェクト
* Assets/Scripts/Main/Addapter
    * アダプタ層
    * UI やインフラ周りなど、仕様変更が多くなる外部とのインターフェースとなるレイヤ
    * レイヤは最も外
    * どこからも参照されず、アダプタ層以内のレイヤを参照する
* Assets/Scripts/Main/Addapter/Infrastructure
    * アダプタ層インフラ関係
    * DB, Network, Cacheなど永続化処理の具象実装
* Assets/Scripts/Main/Addapter/UI
    * アダプタ層 UI 関係
    * シーンマネージャやコンポネントなどUI全般の実装
* Assets/Scripts/Main/DI
    * DI関係
    * アダプタ層からアプリケーション層を呼び出すときにどの具象実装を使うか指定するために使うなど
* Assets/Scripts/Main/Domain
    * ドメイン層
    * 業務ロジックを担当する
    * レイヤは最も内
    * ドメイン層以外のレイヤから参照され、他のレイヤを参照しない
* Assets/Scripts/Main/Domain/[ContextName]
    * `境界づけられたコンテキスト` ごとの階層
    * `ユビキタス言語` の適用範囲
* Assets/Scripts/Main/Domain/[ContextName]/IHogeRepository.cs
    * 永続化機構と接続するための interface
    * interface をドメイン層、具象実装をアダプタ層に実装することで一方通行の参照関係を維持したまま、仕様変更が多くなるインフラ周りの具象実装を切り離す事ができる
* Assets/Scripts/Main/Domain/[ContextName]/Hoge.cs
    * エンティティ
    * 外向けには interface のみを公開し具象実装は隠蔽する
    * 具象実装生成用のファクトリも一緒に定義する
* Assets/Scripts/Main/Domain/[ContextName]/ValueObect.cs
    * 対象のコンテキスト内で使用する `value object` をまとめて定義する
* Assets/Scripts/Main/Domain/DomainError.cs
    * ドメイン層で発生したエラー情報
* Assets/Scripts/Main/Domain/DomainResult.cs
    * ドメイン層の処理結果
* Assets/Scripts/Main/Domain/IEntity.cs
    * エンティティ interface
* Assets/Scripts/Main/Domain/IValueObject.cs
    * value object interface
* Assets/Scripts/Main/UseCase
    * アプリケーション層
    * ドメインロジックを用いてトランザクション整合性を担保する一連の処理をユースケースとして定義
    * レイヤは中間
    * アダプタ層から参照され、ドメイン層を参照する
* Assets/Scripts/Main/UseCase/[ContextName]
    * Domain/[ContextName] に同じ
* Assets/Scripts/Main/UseCase/ApplicationError.cs
    * アプリケーション層で発生したエラー情報
* Assets/Scripts/Main/UseCase/ApplicationResult.cs
    * アプリケーション層の処理結果
* Assets/Scripts/Main/UseCase/IUseCase.cs
    * ユースケース interface

### test

```
Assets/Scripts/Test
    - Editor
        - Domain
        - UseCase
    - Player
```

* Assets/Scripts/Test
    * テストコード
* Assets/Scripts/Test/Editor
    * Editor Test
* Assets/Scripts/Test/Editor/Domain
    * メインプロジェクトに同じ
* Assets/Scripts/Test/Editor/UseCase
    * メインプロジェクトに同じ
* Assets/Scripts/Test/Player
    * Player Test

### unity extension

```
Assets/Scripts/Custom
```

* Assets/Scripts/Custom
    * Unity 拡張全般

