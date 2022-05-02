using Konata.Core.Message;

namespace VanillaForKonata.BotFunction
{
    public static partial class Sys
    {
        public static partial class Help{
            static class HelpPicker {
                static private readonly string OtherTextAtSubMenubPage = "输入标号可以查看某一类帮助\n输入exit可以退出帮助模式\n输入一个点可以回到最开始的页面";
                static private string HelpTextAtDetailPage(int ImmersionFlag)
                {
                    if (ImmersionFlag==1)
                    {
                        return "输入exit可以退出帮助模式\n输入一个点可以回到最开始页面\n该指令支持沉浸模式";
                    }
                    else if(ImmersionFlag==2)
                    {
                        return "输入exit可以退出帮助模式\n输入一个点可以回到最开始页面\n该指令仅支持沉浸模式";
                    }
                    else
                    {
                        return "输入exit可以退出帮助模式\n输入一个点可以回到最开始页面";
                    }
                }
                /// <summary>
                /// Help-帮助文本[Menu]
                /// </summary>
                static public MessageBuilder Help_1 = BuildHelpMessage("帮助", "1.System\n2.图片\n3.小工具", OtherTextAtSubMenubPage);
                /// <summary>
                /// Help-帮助文本-System[Menu]
                /// </summary>
                static public MessageBuilder Help_1_1 = BuildHelpMessage("System", "1.ping\n2.bot状态\n3.开关", OtherTextAtSubMenubPage);
                /// <summary>
                /// Help-帮助文本-System-ping[Detail]
                /// </summary>
                static public MessageBuilder Help_1_1_1 = BuildHelpMessage("Ping", "本指令属于System模块，不可开关\n基本指令：/ping\n如果bot在线会回复你pong，没有实质性作用", HelpTextAtDetailPage(0));
                /// <summary>
                /// Help-帮助文本-System-bot状态[Detail]
                /// </summary>
                static public MessageBuilder Help_1_1_2 = BuildHelpMessage("Bot状态", "本指令属于System模块，不可开关\n基本指令：/v stat\n会回复你bot相关的状态，没有实质性作用", HelpTextAtDetailPage(0));
                /// <summary>
                /// Help-帮助文本-System-开关[Detail]
                /// </summary>
                static public MessageBuilder Help_1_1_3 = BuildHelpMessage("开关", "本指令属于System模块，不可开关\n基本指令：" +
                    "\n1./v module list#用于查看模块的列表及当前开关状况" +
                    "\n2./v module off functionName#关闭一个模块" +
                    "\n3./v module on functionName#开启一个模块" +
                    "\n指令2和3只能群管理员或群主使用" +
                    "\nEg:" +
                    "\n开启复读功能" +
                    "\n (在群主或管理员状态下)/v module on 复读", HelpTextAtDetailPage(0));



                /// <summary>
                /// Help-帮助文本-图片[Menu]
                /// </summary>
                static public MessageBuilder Help_1_2 = BuildHelpMessage("图片", "1.龙图", OtherTextAtSubMenubPage);
                /// <summary>
                /// Help-帮助文本-图片-龙图[Detail]
                /// </summary>
                static public MessageBuilder Help_1_2_1 = BuildHelpMessage("龙图", "本指令属于龙图模块，可通过开关命令控制开关\n基本指令：龙图来\n效果：发送一张龙图并对在场所有马族实体进行高伤害攻击", HelpTextAtDetailPage(0));

                /// <summary>
                /// Help-帮助文本-小工具[Menu]
                /// </summary>
                static public MessageBuilder Help_1_3 = BuildHelpMessage("System", "1.选择困难症", OtherTextAtSubMenubPage);
                /// <summary>
                /// Help-帮助文本-小工具-选择[Detail]
                /// </summary>
                static public MessageBuilder Help_1_3_1 = BuildHelpMessage("选择困难症", "本指令属于选择模块，可通过开关命令控制开关\n基本指令：/v choose 选项1 选项2 选项3 ....\n效果：帮助你进行选择，但是可能结果不是你想要的，仅供参考，bot及开发者不对选择负任何责任", HelpTextAtDetailPage(0));
            }
        }
    }
}
