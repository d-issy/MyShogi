﻿using MyShogi.Model.Resource.Images;
using MyShogi.Model.Common.ObjectModel;
using SPRITE = MyShogi.Model.Resource.Images.SpriteManager;
using MyShogi.Model.Common.Utility;
using MyShogi.Model.Shogi.Core;

namespace MyShogi.View.Win2D
{
    /// <summary>
    /// Animator関連はここに書く。
    /// </summary>
    public partial class GameScreenControl
    {
        /// <summary>
        /// Animator管理クラス
        /// </summary>
        private AnimatorManager animatorManager = new AnimatorManager();

        #region PropertyChangedEventHandlers
        /// <summary>
        /// 対局開始
        /// </summary>
        /// <param name="args"></param>
        private void GameStartEventHandler(PropertyChangedEventArgs args)
        {
            animatorManager.ClearAnimator(); // 他のanimatorをすべて削除

            // これだけでアニメーションできる。便利すぎ。

            animatorManager.AddAnimator(new Animator(
                elapsed => {
                    // 「対局開始」
                    DrawSprite(game_start_pos , SPRITE.GameStart());

                    var handicapped = (bool)args.value;

                    var black = handicapped ? SPRITE.GameShitate() : SPRITE.GameBlack();
                    var white = handicapped ? SPRITE.GameUwate()   : SPRITE.GameWhite();

                    if (gameServer.BoardReverse)
                        Utility.Swap(ref black , ref white);

                    // 「先手」/「下手」
                    DrawSprite(game_black_pos, black);
                    // 「後手」/「上手」
                    DrawSprite(game_white_pos, white);

                }, false , 0 , 2000
            ));
        }

        /// <summary>
        /// 対局終了
        /// </summary>
        /// <param name="args"></param>
        private void GameEndEventHandler(PropertyChangedEventArgs args)
        {
            animatorManager.ClearAnimator(); // 他のanimatorをすべて削除

            // 対局結果から、「勝ち」「負け」「引き分け」のいずれかを表示する。
            var sprite = SPRITE.GameResult((MoveGameResult)args.value);
            if (sprite != null)
            {
                // 1秒後以降に表示する。(すぐに表示されるとちょっと邪魔)
                animatorManager.AddAnimator(new Animator(
                    elapsed => { DrawSprite(game_result_pos, sprite); }, false, 1000, 3500
                ));
            }
            else
            {
                // 「対局終了」の素材を表示する。
                // 1秒後以降に表示する。(すぐに表示されるとちょっと邪魔)
                animatorManager.AddAnimator(new Animator(
                    elapsed => { DrawSprite(game_start_pos, SPRITE.GameEnd()); }, false, 1000, 3500
                ));
            }

        }
        #endregion
    }
}