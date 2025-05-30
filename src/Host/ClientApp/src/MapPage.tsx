import { useIndexQuery } from './entities/MapData';
import ErrorField from './shared/ErrorField';
import LoadingCard from './shared/LoadingCard';
import DefaultErrorCard from './shared/DefaultErrorCard';
import GameMap, { type GameMapElement } from './widgets/GameMap';

function HomePage() {
  const { data, isLoading, error } = useIndexQuery();

  const mapElements: GameMapElement[] | undefined = data == undefined
    ? undefined
    :  Object.entries(data).map(([_, value]) : GameMapElement => {
        return {
          id: 20 + value.name.length * 2,
          name: value.name,
          description: value.info.join(),
          lat: value.name.length * 2 + value.info.length / 10 - 70,
          lng: value.name.length * 2 + value.info.length / 10 - 75,
          links: [
            {
              onClick: () => { },
              name: 'Выбрать'
            },
            {
              onClick: () => { },
              name: 'Подробнее'
            },
          ]
        }
      });

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error == undefined && data != undefined && mapElements != undefined
          ? <GameMap mapElements={mapElements!} />
          : <DefaultErrorCard />}
    </>
  )
}

export default HomePage