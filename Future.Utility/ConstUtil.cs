using System.Collections.Generic;

namespace Future.Utility
{
    /// <summary>
    /// 常量集合
    /// </summary>
    public static class ConstUtil
    {
        public readonly static string LETTERUSERKEY = "LetterUserKey_{0}";

        /// <summary>
        /// 7天对应的秒
        /// </summary>
        public readonly static int SevenDaySeconds = 604800;

        /// <summary>
        /// 个性标签默认值
        /// </summary>
        public readonly static List<string> CHARACTER_TAG_LIST =new List<string>() {"厚道","靠谱","文艺","安静", "随性", "理想主义", "萌萌哒", "女汉子", "强迫症", "拖延症", "极品吃货", "叫我逗比", "双重人格", "喜欢简单", "敢爱敢恨", "选择恐惧症", "宅", "局气", "有面儿", "讲义气", "女友永远是对的", "AJ控", "大叔控", "马甲线", "长发及腰", "健谈", "叛逆", "热血"};

        /// <summary>
        /// 运动爱好默认标签
        /// </summary>
        public readonly static List<string> SPORT_TAG_LIST = new List<string>() { "游泳","跑步", "单车", "足球", "滑板", "滑雪", "瑜伽", "篮球", "乒乓球", "羽毛球", "网球", "高尔夫", "台球", "舞蹈", "街舞", "健身房", "射箭", "击剑", "射击", "拳击", "爬山", "骑马", "郊游", "暴走", "睡觉"};

        /// <summary>
        /// 音乐爱好默认标签
        /// </summary>
        public readonly static List<string> MUSIC_TAG_LIST = new List<string>() { "流行", "摇滚", "电子", "R&B", "嘻哈", "爵士", "布鲁斯", "金属", "欧美", "日韩", "轻音乐", "古典", "乡村", "校园名谣", "60年代经典", "80年代金典"};

        /// <summary>
        /// 食物爱好标签
        /// </summary>
        public readonly static List<string> FOOD_TAG_LIST = new List<string>() { "火锅", "麻辣烫", "薯条", "北京烤鸭", "港式早茶", "烤串", "麻辣香锅", "麻小", "生煎包", "卤肉饭", "寿司", "生鱼片", "日本拉面", "日式铁板烧", "石锅拌饭", "韩国烤肉", "牛排", "意大利面", "披萨", "汉堡", "美式炸鸡", "土耳其烤肉", "素食", "提拉米苏", "慕斯蛋糕", "奶酪", "巧克力", "冰激凌"};

        /// <summary>
        /// 电影默认标签
        /// </summary>
        public readonly static List<string> MOVIE_TAG_LIST = new List<string>() { "肖申克的救赎", "霸王别姬", "教父", "阿甘正传", "泰坦尼克号", "这个杀手不太冷", "盗梦空间", "黑客帝国", "蝙蝠侠", "低俗小说", "搏击俱乐部", "海上钢琴师", "触不可及", "千与千寻", "忠犬八公的故事", "罗马假日", "乱世佳人", "当幸福来敲门", "辛德勒的名单", "天使爱美丽", "两杆大烟枪", "闻香识女人", "死亡诗杜", "美丽心灵", "魔戒三部曲", "狮子王", "哈利波特", "大话西游", "喜剧之王", "东邪西毒", "阳光灿烂的日子","我的野蛮女友", "初恋这件小事", "重庆森林", "春光乍泄", "心灵捕手", "放牛班的春天"};

        /// <summary>
        /// 旅游默认标签
        /// </summary>
        public readonly static List<string> TRAVEL_TAG_LIST = new List<string>() { "成都", "新加坡", "济州岛", "韩国", "日本", "桂林", "三亚", "丽江", "大理", "香格里拉", "西藏", "鼓浪屿", "张家界", "九寨沟", "台湾", "北海道", "巴厘岛", "普吉岛", "长滩岛", "马来西亚", "泰国", "菲律宾", "印度尼西亚", "印度", "越南", "尼泊尔", "迪拜", "土耳其", "美国", "澳大利亚", "英国", "法国", "意大利","西班牙","古巴", "新西兰", "俄罗斯", "埃及"};
    }
}
