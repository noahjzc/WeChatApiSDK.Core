﻿using FluentValidation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace WeChatApiSDK.Core.DTO.Menu
{
    public class CreateMenuRequest : IRequestModel
    {
        /// <summary>
        /// 一级菜单数组，个数应为1~3个
        /// </summary>
        [JsonProperty("button")]
        public IEnumerable<RootButtonItem> Button { get; set; }

        public class RootButtonItem : ButtonItem
        {
            /// <summary>
            /// 二级菜单数组，个数应为1~5个
            /// </summary>
            [JsonProperty("sub_button")]
            public IEnumerable<ButtonItem> SubButton { get; set; }
        }

        public class ButtonItem
        {
            /// <summary>
            /// 菜单的响应动作类型，view表示网页类型，click表示点击类型，miniprogram表示小程序类型
            /// </summary>
            [JsonProperty("type")]
            public string Type { get; set; }

            /// <summary>
            /// 菜单标题，不超过16个字节，子菜单不超过60个字节
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// click等点击类型必须
            /// 菜单KEY值，用于消息接口推送，不超过128字节
            /// </summary>
            [JsonProperty("key")]
            public string Key { get; set; }

            /// <summary>
            /// view、miniprogram类型必须
            /// 网页 链接，用户点击菜单可打开链接，不超过1024字节。 type为miniprogram时，不支持小程序的老版本客户端将打开本url。
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; }

            /// <summary>
            /// media_id类型和view_limited类型必须
            /// 调用新增永久素材接口返回的合法media_id
            /// </summary>
            [JsonProperty("media_id")]
            public string MediaId { get; set; }

            /// <summary>
            /// miniprogram类型必须
            /// 小程序的appid（仅认证公众号可配置）
            /// </summary>
            [JsonProperty("appid")]
            public string AppId { get; set; }

            /// <summary>
            /// miniprogram类型必须
            /// 小程序的页面路径
            /// </summary>
            [JsonProperty("pagepath")]
            public string PagePath { get; set; }
        }

        public override bool Validate()
        {
            return base.Validate(new CreateMenuRequestValidator());
        }
    }

    /// <summary>
    /// 请注意，3到8的所有事件，仅支持微信iPhone5.4.1以上版本，和Android5.4以上版本的微信用户，旧版本微信用户点击后将没有回应，开发者也不能正常接收到事件推送。
    /// 9和10，是专门给第三方平台旗下未微信认证（具体而言，是资质认证未通过）的订阅号准备的事件类型，它们是没有事件推送的，能力相对受限，其他类型的公众号不必使用。
    /// </summary>
    public static class MenuTypeEnum
    {
        /// <summary>
        /// 点击推事件用户点击click类型按钮后，微信服务器会通过消息接口推送消息类型为event的结构给开发者（参考消息接口指南），并且带上按钮中开发者填写的key值，开发者可以通过自定义的key值与用户进行交互
        /// </summary>
        public const string Click = "click";

        /// <summary>
        /// 小程序类型
        /// </summary>
        public const string MiniProgram = "miniprogram";

        /// <summary>
        /// 跳转URL用户点击view类型按钮后，微信客户端将会打开开发者在按钮中填写的网页URL，可与网页授权获取用户基本信息接口结合，获得用户基本信息。
        /// </summary>
        public const string View = "view";

        /// <summary>
        /// 扫码推事件用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后显示扫描结果（如果是URL，将进入URL），且会将扫码的结果传给开发者，开发者可以下发消息。
        /// </summary>
        public const string ScancodePush = "scancode_push";

        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后，将扫码的结果传给开发者，同时收起扫一扫工具，然后弹出“消息接收中”提示框，随后可能会收到开发者下发的消息。
        /// </summary>
        public const string ScancodeWaitMsg = "scancode_waitmsg";

        /// <summary>
        /// 弹出系统拍照发图用户点击按钮后，微信客户端将调起系统相机，完成拍照操作后，会将拍摄的相片发送给开发者，并推送事件给开发者，同时收起系统相机，随后可能会收到开发者下发的消息。
        /// </summary>
        public const string PicSysPhoto = "pic_sysphoto";

        /// <summary>
        /// 弹出拍照或者相册发图用户点击按钮后，微信客户端将弹出选择器供用户选择“拍照”或者“从手机相册选择”。用户选择后即走其他两种流程。
        /// </summary>
        public const string PicPhotoOrAlbum = "pic_photo_or_album";

        /// <summary>
        /// 弹出微信相册发图器用户点击按钮后，微信客户端将调起微信相册，完成选择操作后，将选择的相片发送给开发者的服务器，并推送事件给开发者，同时收起相册，随后可能会收到开发者下发的消息。
        /// </summary>
        public const string PicWeixin = "pic_weixin";

        /// <summary>
        /// 弹出地理位置选择器用户点击按钮后，微信客户端将调起地理位置选择工具，完成选择操作后，将选择的地理位置发送给开发者的服务器，同时收起位置选择工具，随后可能会收到开发者下发的消息。
        /// </summary>
        public const string LocationSelect = "location_select";

        /// <summary>
        /// 下发消息（除文本消息）用户点击media_id类型按钮后，微信服务器会将开发者填写的永久素材id对应的素材下发给用户，永久素材类型可以是图片、音频、视频、图文消息。请注意：永久素材id必须是在“素材管理/新增永久素材”接口上传后获得的合法id。
        /// </summary>
        public const string MediaId = "media_id";

        /// <summary>
        /// 跳转图文消息URL用户点击view_limited类型按钮后，微信客户端将打开开发者在按钮中填写的永久素材id对应的图文消息URL，永久素材类型只支持图文消息。请注意：永久素材id必须是在“素材管理/新增永久素材”接口上传后获得的合法id。
        /// </summary>
        public const string ViewLimited = "view_limited";
    }


    internal class CreateMenuRequestValidator : AbstractValidator<CreateMenuRequest>
    {
        public CreateMenuRequestValidator()
        {
            RuleFor(x => x.Button.Count()).InclusiveBetween(1, 3);
            RuleForEach(x => x.Button).SetValidator(new RootButtonItemValidator());
        }
    }

    internal class RootButtonItemValidator : AbstractValidator<CreateMenuRequest.RootButtonItem>
    {
        public RootButtonItemValidator()
        {
            Include(new ButtonItemValidator());

            When(x => x.SubButton != null, () =>
            {
                RuleFor(x => x.SubButton.Count()).InclusiveBetween(1, 5);
                RuleForEach(x => x.SubButton).SetValidator(new ButtonItemValidator());
            });
        }
    }

    internal class ButtonItemValidator : AbstractValidator<CreateMenuRequest.ButtonItem>
    {
        public ButtonItemValidator()
        {
            // RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("按钮类型不能为空");
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(x => "按钮名称不能为空");

            When(x => x.Type == MenuTypeEnum.Click, () =>
            {
                RuleFor(x => x.Key).MaximumLength(128).NotEmpty().NotNull().WithMessage(x => "按钮类型为Click时，Key不能为空");
            });

            When(x => x.Type == MenuTypeEnum.View, () =>
            {
                RuleFor(x => x.Url).NotNull().NotEmpty().MaximumLength(1024).WithMessage(x => "按钮类型为View时，Url不能为空，且长度不大于1024");
            });

            When(x => x.Type == MenuTypeEnum.MiniProgram, () =>
            {
                RuleFor(x => x.Url).NotNull().NotEmpty().MaximumLength(1024).WithMessage(x => "按钮类型为MiniProgram时，Url不能为空，且长度不大于1024");
                RuleFor(x => x.AppId).NotNull().NotEmpty().WithMessage(x => "按钮类型为MiniProgram时，AppId不能为空");
                RuleFor(x => x.PagePath).NotNull().NotEmpty().WithMessage(x => "按钮类型为MiniProgram时，PagePath不能为空");
            });

            When(x => (x.Type == MenuTypeEnum.MediaId || x.Type == MenuTypeEnum.ViewLimited),
                () =>
                {
                    RuleFor(x => x.MediaId).NotNull().NotEmpty();
                });
        }
    }
}
