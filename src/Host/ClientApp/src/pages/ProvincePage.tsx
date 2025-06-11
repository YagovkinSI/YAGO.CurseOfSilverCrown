import { Divider, Typography } from '@mui/material';
import YagoCard from '../shared/YagoCard';
import { type MapElement } from '../entities/MapData';
import { useIndexQuery } from '../entities/MapData';
import { useParams } from 'react-router-dom';
import type YagoEnity from '../entities/YagoEnity';
import ErrorField from '../shared/ErrorField';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import LoadingCard from '../shared/LoadingCard';

const ProvincePage: React.FC = () => {
    const { id } = useParams();
    const idAsNumber = id == 'up' || id == 'down'
        ? -1
        : id == null
            ? 0
            : parseInt(id, 10) || 0;

    const { data, isLoading, error } = useIndexQuery();
    const unknownEarthEntity : YagoEnity = { id: -1, name: "Неигровая провинция", type: "Unknown" };
    const unknownEarthMapElement : MapElement = {
        yagoEntity: unknownEarthEntity, 
        info: [],
        colorStr: 'gray'
    } 
    const province = idAsNumber == -1
        ? unknownEarthMapElement
        : data?.[`${idAsNumber}`];

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