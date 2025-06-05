using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using LearnApp.Control;

namespace LearnApp.Win.TempSelector
{
    /// <summary>
    /// Chat数据模板选择器
    /// </summary>
    public class ChatTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ChatContent content = item as ChatContent;
            if (content != null)
            {
                DataTemplate template;
                //判定数据实体类型选择不同的数据模板
                if (content.Type == 1)
                {
                    template = Application.Current.TryFindResource("chatother") as DataTemplate;
                }
                else
                {
                    template = Application.Current.TryFindResource("chatowner") as DataTemplate;
                }
                return template;
            }

            return null;
        }
    }
}
