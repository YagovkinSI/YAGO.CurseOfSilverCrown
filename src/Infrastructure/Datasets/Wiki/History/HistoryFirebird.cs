using YAGO.World.Domain.Common;
using YAGO.World.Domain.Wiki;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.Wiki
{
    public static class HistoryFirebird
    {
        public static WikiArticle Get()
        {
            return new(WikiArticleConstants.LifeFirebird, WikiSection.History, 3, new DisplayInfo(
                "Термоядерный реактор «Жар-птица»",
                ImageSet.Spaceship,
                [
                    "2043 год, Троицк, Россия.",
                    "На основе расчётов 2032 года и опыта, накопленного при строительстве демонстратора в Хэфэе, специалисты Курчатовского института и Госкорпорации «Росатом» завершили создание первого в мире компактного термоядерного реактора «Жар-птица». " +
                    "Установка размером с футбольное поле — значительно компактнее международных проектов вроде ITER — работает на смеси дейтерия и гелия-3, доставленного с лунной базы.",
                    "К середине 2040-х годов на основе этой технологии появились первые термоядерные двигатели для межпланетных кораблей. В настоящее время такие двигатели стали стандартом для дальних перелётов в Пояс и к внешним планетам."
                ]));
        }
    }
}