import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import { useGetCurrentUserQuery } from '../entities/CurrentUser';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { Typography } from '@mui/material';
import { ToGameDate } from '../features/GameDateCreator';

const HomePage: React.FC = () => {
  const { data, isLoading, error } = useGetCurrentUserQuery();

  const getUtcDateString = (date = new Date()): string => {
    const year = date.getUTCFullYear();
    const month = String(date.getUTCMonth() + 1).padStart(2, '0');
    const day = String(date.getUTCDate()).padStart(2, '0');
    return `${year}-${month}-${day}T00:00:00Z`;
  }
  const utcDateString = getUtcDateString();

  const renderStartCardContent = () => {
    return (
      <>
        <Typography textAlign="justify" gutterBottom>
          Идёт {ToGameDate(utcDateString)}
        </Typography>
        <Typography variant='h5' textAlign="justify" gutterBottom>
          На материке Исей разворачивается новая эпоха!
        </Typography>
        <Typography textAlign="justify" gutterBottom>
          Местные народы, перенявшие технологии эльниров, уже строят первые города-государства. Сейчас решается, кто возвысится над другими – мудрый стратег, искусный дипломат или бесстрашный завоеватель.
        </Typography>
      </>
    )
  }

  const renderAuthorizationButtons = () => {
    return (
      <>
        <ButtonWithLink to={'/Identity/Account/Register'} text={'Регистрация'} />
        <ButtonWithLink to={'/Identity/Account/Login'} text={'Авторизация'} />
      </>
    )
  }

  const renderGuestCard = () => {
    return (
      <YagoCard
        title='Добро пожаловать в мир Яго!'
        image={'/assets/images/pictures/homepage.jpg'}
      >
        {renderStartCardContent()}
        <Typography textAlign="justify" gutterBottom>
          Твой ход! Возглавь одно из молодых государств и поведи его к величию – через торговлю, войны или хитросплетения политики.
        </Typography>
        <Typography textAlign="justify" gutterBottom>
          Присоединяйся к игре – твои решения изменят ход истории Исея!
        </Typography>
        <ButtonWithLink to={'/app/history'} text={'История мира'} />
        {
          data?.isAuthorized
            ? <ButtonWithLink to={'/app/map'} text={'Выбрать фракцию на карте'} />
            : renderAuthorizationButtons()          
        }

      </YagoCard>
    )
  }

  const renderUserCard = () => {
    return (
      <YagoCard
        title={`Добро пожаловать, ${data?.user?.userName}!`}
        image={'/assets/images/pictures/homepage.jpg'}
      >
        {renderStartCardContent()}
        <ButtonWithLink to={'/app/history'} text={'История мира'} />
        <ButtonWithLink to={'/Domain'} text={'К владению'} />
      </YagoCard>
    )
  }

  const renderCard = () => {
    return data?.faction == undefined
      ? renderGuestCard()
      : renderUserCard();
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error == undefined && data != undefined
          ? renderCard()
          : <DefaultErrorCard />}
    </>
  )
}

export default HomePage