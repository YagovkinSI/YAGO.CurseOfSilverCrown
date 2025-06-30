import YagoCard from '../shared/YagoCard';
import type HistoryEvent from '../entities/HistoryEvent';
import { HistoryEventLevelList } from '../entities/HistoryEvent';
import HistoryEventCard from '../widgets/HistoryEventCard';
import { Typography } from '@mui/material';

const HomePage: React.FC = () => {

  const events: HistoryEvent[] = [
    {
      id: 1,
      level: HistoryEventLevelList.World,
      entityId: 0,
      dateTime: '2019-01-01T00:00:00Z',
      shortText: 'Эльниры открыли тайну обработки меди, положив начало Медному веку. Пламя их горнов осветило путь к технологическому превосходству, а первые металлические орудия навсегда изменили быт и военное искусство.'
    },
    {
      id: 2,
      level: HistoryEventLevelList.World,
      entityId: 0,
      dateTime: '2022-05-09T00:00:00Z',
      shortText: 'Эльниры освоили искусство кораблестроения, создав суда, покорившие морские дали. Так началась эпоха Великих морских странствий, открывшая новую главу в их истории.'
    },
    {
      id: 3,
      level: HistoryEventLevelList.World,
      entityId: 0,
      dateTime: '2023-10-20T00:00:00Z',
      shortText: 'Эльнирские мореходы проложили путь в Триморье, достигнув берегов континентов Исей, Даджи и Нахум. Первые контакты с местными народами навсегда изменили судьбы всех цивилизаций региона.'
    },
    {
      id: 4,
      level: HistoryEventLevelList.World,
      entityId: 0,
      dateTime: '2024-04-29T00:00:00Z',
      shortText: 'Великая морская экспедиция эльниров достигла окраин континента Ирмар, впервые ступив на эти земли и раздвинув границы известного мира.'
    },
    {
      id: 5,
      level: HistoryEventLevelList.World,
      entityId: 0,
      dateTime: '2024-07-15T00:00:00Z',
      shortText: 'Основана Жемчужная Гавань – город, ставший колыбелью новой цивилизации. Именно с этого момента большинство государств Триморья начинает своё летосчисление.'
    },
  ]

  return (
    <YagoCard title='Мир Яго'>
      <Typography variant="h5" sx={{ textAlign: 'left', marginBottom: '0.5rem' }}>Важнейшие события мира:</Typography>
      <div>
        {events.map(e => {
          return <HistoryEventCard key={e.id} event={e} />
        })}
      </div>
    </YagoCard>
  )
}

export default HomePage