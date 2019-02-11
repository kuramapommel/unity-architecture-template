namespace Domain.Player
{
    /// <summary>
    /// プレイヤーリポジトリ
    /// </summary>
    /// interface をドメイン層に定義し、具象実装をアダプター層に実装することで
    /// 結果として api や store など外部モジュールに依存しやすいインフラ周りを
    /// 一方通行の参照関係を維持したまま切り離すことができる
    public interface IPlayerRepository
    {
        /// <summary>
        /// プレイヤーidを元にプレイヤーを検索する
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="id">Identifier.</param>
        IPlayer FindById(PlayerId id);

        /// <summary>
        /// プレイヤーを保存する
        /// </summary>
        /// <returns>The save.</returns>
        /// <param name="player">Player.</param>
        IPlayer Save(IPlayer player);
    }
}
