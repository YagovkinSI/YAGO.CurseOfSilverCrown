import { useIndexQuery } from './entities/MapData';
import ErrorField from './shared/ErrorField';
import LoadingCard from './shared/LoadingCard';
import DefaultErrorCard from './shared/DefaultErrorCard';
import GameMap, { type GameMapElement } from './widgets/GameMap';

function HomePage() {
  const { data, isLoading, error } = useIndexQuery();

  const mapElements: GameMapElement[] | undefined = data == undefined
    ? undefined
    :  Object.entries(data).map(([key, value]) : GameMapElement => {
        return {
          id: key,
          name: value.name,
          description: value.info.join("\r\n"),
          color: value.colorStr
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