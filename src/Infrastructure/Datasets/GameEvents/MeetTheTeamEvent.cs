using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class MeetTheTeamEvent
    {
        private const string Id = GameEventConstants.MeetTheTeam;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                { "#default", new GameAction(
                    effects: [],
                    newEventCodes: [],
                    displayInfoResult: GetEpilog()) } };
            return new(
                code: Id,
                eventType: EventType.Urgent,
                eventOccurrenceOptions,
                slides: GetPrologSlides(),
                actions: changeList);
        }

        private static Slide[] GetPrologSlides()
        {
            return [
                GetSlide0(),
                GetSlide1(),
                GetSlide2(),
                GetSlide3(),
                GetSlide4()];
        }

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Ваша команда",
                imageName: ImageSet.CaptainHall,
                text: new string[]
                {
                    "Консорциум подобрал вам команду — четыре специалиста, каждый в своей области.",
                    "Познакомьтесь с ними, прежде чем приступить к работе."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Камилла — администратор"),
                    SlideButton.GetButtonToSlide($"{Id}_2", "Кассиус — финансист"),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Лиен — инженер станции"),
                    SlideButton.GetButtonToSlide($"{Id}_4", "Дариус — социальный советник"),
                    SlideButton.GetCloseNewsButton(Id, "Завершить знакомство")]);
        }

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Администратор",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Камилла Селезнева, 34 года. Администратор.",
                    "Родилась в Новосибирске, отец работал на станции «Рубин». Опыт управления, связи на большинстве " +
                    "крупных станций Пояса. Знает Пояс изнутри.",
                    "Координирует работу станции, отвечает за связь с Консорциумом и замещает правителя " +
                    "в его отсутствие. Решает задачи, которые не входят в компетенцию других советников."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_0", "Назад", kind: SlideButtonKind.Return)]);
        }

        private static Slide GetSlide2()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Финансист",
                imageName: ImageSet.Cassius,
                text: new string[]
                {
                    "Кассиус Морн аль-Хаким, 36 лет. Финансист и юрист.",
                    "Родился в Дубае, степень Лондонской школы экономики. Двенадцать лет в корпоративном праве " +
                    "и налоговом консалтинге. Холодный ум, знает устав Консорциума лучше, чем сам Консорциум.",
                    "Управляет бюджетом, налогами, контрактами и отчётностью перед Консорциумом. " +
                    "Отвечает за юридическую защиту колонии."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_0", "Назад", kind: SlideButtonKind.Return)]);
        }

        private static Slide GetSlide3()
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Инженер станции",
                imageName: ImageSet.Lien,
                text: new string[]
                {
                    "Лиен Чжан, 30 лет. Инженер станции.",
                    "Родилась в Циндао, в 19 лет улетела на Луну по программе RAS. Прошла путь от разнорабочей " +
                    "до старшего инженера. Ценит безопасность выше эффективности.",
                    "Отвечает за реактор, системы жизнеобеспечения и техническое состояние станции. " +
                    "Следит за безопасностью и предотвращает аварии."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_0", "Назад", kind: SlideButtonKind.Return)]);
        }

        private static Slide GetSlide4()
        {
            return new Slide(
                id: $"{Id}_4",
                title: "Социальный советник",
                imageName: ImageSet.Darius,
                text: new string[]
                {
                    "Дариус «Док» Уэбб, 42 года. HR и миграционный контролёр.",
                    "Родился в Хьюстоне, пятнадцать лет служил в миграционной службе ОПЗ на Луне. " +
                    "Считает, что каждый заслуживает шанса.",
                    "Отвечает за найм, удержание колонистов и внутренний климат. " +
                    "Решает конфликты между работниками."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_0", "Назад", kind: SlideButtonKind.Return)]);
        }

        private static DisplayInfo? GetEpilog()
        {
            return new DisplayInfo(
                "Знакомство завершено",
                ImageSet.CaptainHall,
                [
                    "Команда собрана и готова приступить к работе. Вы можете связаться с каждым советником в любой момент."
                ]);
        }
    }
}