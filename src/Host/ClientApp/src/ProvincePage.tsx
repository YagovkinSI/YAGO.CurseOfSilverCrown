import { Typography } from '@mui/material';
import YagoCard from './shared/YagoCard';
import { useIndexQuery } from './entities/MapData';
import ErrorField from './shared/ErrorField';
import LoadingCard from './shared/LoadingCard';
import DefaultErrorCard from './shared/DefaultErrorCard';
import { useParams } from 'react-router-dom';
import ButtonWithLink from './shared/ButtonWithLink';

const ProvincePage: React.FC = () => {
    const { id } = useParams();
    const idAsNumber = id == null
        ? 0
        : parseInt(id, 10) || 0;

    const { data, isLoading, error } = useIndexQuery();
    const province = data?.[`${idAsNumber}`];

    const renderCard = () => {
        return (
            <YagoCard image={undefined}>
                <Typography variant="h1" gutterBottom>{province?.name}</Typography>
                {province?.info.map(i => 
                    i == '<hr>'
                        ? <Typography></Typography>
                        : <Typography>{i}</Typography>
                )}
                <ButtonWithLink to={'/app/map/'} text={'Закрыть'} />
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading
                ? <LoadingCard />
                : error == undefined && data != undefined && province != undefined
                    ? renderCard()
                    : <DefaultErrorCard />}
        </>
    )
}

export default ProvincePage