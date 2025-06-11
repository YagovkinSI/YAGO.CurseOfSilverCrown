import { Divider, Typography } from '@mui/material';
import YagoCard from '../shared/YagoCard';
import { useIndexQuery } from '../entities/MapData';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { useParams } from 'react-router-dom';

const ProvincePage: React.FC = () => {
    const { id } = useParams();
    const idAsNumber = id == null
        ? 0
        : parseInt(id, 10) || 0;

    const { data, isLoading, error } = useIndexQuery();
    const province = data?.[`${idAsNumber}`];

    const renderCard = () => {
        return (
            <YagoCard 
                title={province?.yagoEntity.name ?? 'Неизвестная провинция'} 
                titleLink={ province == undefined || province.yagoEntity.type == 'Unknown' 
                    ? undefined 
                    : `/Organizations/Details/${province.yagoEntity.id}`}
            >
                <Divider />
                {province?.info.map(i =>
                    i == '<hr>'
                        ? <Divider />
                        : <Typography textAlign="justify" gutterBottom>{i}</Typography>
                )}
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