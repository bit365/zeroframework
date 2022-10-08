import { DefaultFooter } from '@ant-design/pro-layout';
import {useIntl} from 'umi'

export default () => (
  <DefaultFooter
    copyright= {useIntl().formatMessage({id:'site.title'})}
    links={[]}
  />
);
