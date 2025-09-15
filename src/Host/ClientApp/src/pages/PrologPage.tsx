import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { Typography } from '@mui/material';

const PrologPage: React.FC = () => {

  const isLoading = false;
  const error = undefined;

  const renderCard = () => {
    return (
      <YagoCard
        title={'Пролог'}
        image={'/assets/images/pictures/vulcano.jpg'}
        headerButtonsAccess={false}
      >
        <Typography textAlign="justify" gutterBottom>
          Гнев богов обрушился на Море Сотен Островов. Великий вулкан разорвал небо, погребая под пеплом и волнами цветущие острова эльниров. Ядовитый туман окутал побережье Триморья, порты замерли, а торговые пути опустели. Началась новая эпоха.
        </Typography>
        <Typography textAlign="justify" gutterBottom>
          Вы — наместник небольшого полиса. Вы управляли им мудро, пока ваш покровитель, архонт-эльнир Теларион, вёл переговоры на островах. Теперь вести с моря не поступают, а среди рыбаков ходят слухи, что его великую трирему разметала буря у самого мыса Гнева.
        </Typography>
        <ButtonWithLink to={'/app/map'} text={'Выбрать владение на карте'} />
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error == undefined
          ? renderCard()
          : <DefaultErrorCard />}
    </>
  )
}

export default PrologPage