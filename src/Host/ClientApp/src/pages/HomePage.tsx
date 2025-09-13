import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import { useGetCurrentUserQuery } from '../entities/CurrentUser';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { Typography } from '@mui/material';

const HomePage: React.FC = () => {
  const { data, isLoading, error } = useGetCurrentUserQuery();

  const renderGuestContent = () => {
    return (
      <>
        <Typography textAlign="center" gutterBottom>
          Хитрость, сила или дипломатия? Выбери путь к власти.
        </Typography>
        <ButtonWithLink to={'/registration'} text={'Авторизация'} />
      </>
    )
  }

  const renderUserContent = () => {
    return (
      <>
        <Typography textAlign="center" gutterBottom>
          {data!.user!.userName}, твои владения ждут своего правителя.
        </Typography>
        {
          data?.faction != undefined
            ? <ButtonWithLink to={'/Domain'} text={'К владению'} />
            : <ButtonWithLink to={'/app/map'} text={'Выбрать владение на карте'} />
        }
      </>
    )
  }

  const renderCard = () => {
    return (
      <YagoCard
        title={'Yago World'}
        image={'/assets/images/pictures/homepage.jpg'}
        headerButtonsAccess={false}
      >
        {data?.isAuthorized
          ? renderUserContent()
          : renderGuestContent()}
      </YagoCard>
    )
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