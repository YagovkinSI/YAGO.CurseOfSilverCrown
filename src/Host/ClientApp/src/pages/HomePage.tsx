import YagoCard from '../shared/YagoCard';
import type HistoryEvent from '../entities/HistoryEvent';
import { HistoryEventLevelList } from '../entities/HistoryEvent';
import HistoryEventCard from '../widgets/HistoryEventCard';
import ButtonWithLink from '../shared/ButtonWithLink';

const HomePage: React.FC = () => {

  const events: HistoryEvent[] = [
    {
      id: 5,
      level: HistoryEventLevelList.World,
      entityId: 0,
      dateTime: '2024-07-15T00:00:00Z',
      shortText: 'Основана Жемчужная Гавань – город, ставший колыбелью новой цивилизации. Именно от этого момента большинство государств Триморья сейчас ведёт своё летосчисление.'
    },
    {
      id: 4,
      level: HistoryEventLevelList.World,
      entityId: 0,
      dateTime: '2024-04-29T00:00:00Z',
      shortText: 'Великая морская экспедиция эльниров достигла окраин континента Ирмар, впервые ступив на эти земли и раздвинув границы известного мира.'
    },
    {
      id: 3,
      level: HistoryEventLevelList.World,
      entityId: 0,
      dateTime: '2023-10-20T00:00:00Z',
      shortText: 'Эльнирские мореходы проложили путь в Триморье, достигнув берегов континентов Исей, Даджи и Нахум. Первые контакты с местными народами навсегда изменили судьбы всех цивилизаций региона.'
    },
    {
      id: 2,
      level: HistoryEventLevelList.World,
      entityId: 0,
      dateTime: '2022-05-09T00:00:00Z',
      shortText: 'Эльниры освоили искусство кораблестроения, создав суда, покорившие морские дали. Так началась эпоха Великих морских странствий, открывшая новую главу в их истории.'
    },
    {
      id: 1,
      level: HistoryEventLevelList.World,
      entityId: 0,
      dateTime: '2019-01-01T00:00:00Z',
      shortText: 'Эльниры открыли тайну обработки меди, положив начало Медному веку. Пламя их горнов осветило путь к технологическому превосходству, а первые металлические орудия навсегда изменили быт и военное искусство.'
    },
  ]

  const getUtcDateString = (date = new Date()): string => {
    const year = date.getUTCFullYear();
    const month = String(date.getUTCMonth() + 1).padStart(2, '0');
    const day = String(date.getUTCDate()).padStart(2, '0');
    return `${year}-${month}-${day}T00:00:00Z`;
  }

  const playNowEvent: HistoryEvent = {
    id: 0,
    level: HistoryEventLevelList.World,
    entityId: 0,
    dateTime: getUtcDateString(),
    shortText: 'Первые государства людей рождаются из смеси эльнирских технологий и амбиций местных вождей. Твоя очередь вершить историю – присоединяйся!'
  }

  return (
    <YagoCard title='Мир Яго'>
      <HistoryEventCard event={playNowEvent}>
        <ButtonWithLink to={'/app/factions'} text={'Список фракций'} />
      </HistoryEventCard>
      {events.map(e => {
          return <HistoryEventCard key={e.id} event={e} />
        })}
    </YagoCard>
  )
}

export default HomePage