using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    interface IConfigRules
    {
        /// <summary>
        /// 配置modbusTcp规则
        /// </summary>
        /// <param name="mtrf">modbusTcp规则实例</param>
        /// <param name="add_delete">添加规则或者删除规则(值为true时添加规则)</param>
        bool ConfigModbusTcpRules(ModbusTcpRulesForm mtrf,bool add_delete);

        /// <summary>
        /// 配置OPC规则
        /// </summary>
        /// <param name="orf">OPC规则实例</param>
        /// <param name="add_delete">添加规则或者删除规则(值为true时添加规则)</param>
        bool ConfigOPCRules(OPCRulesForm orf, bool add_delete);

        /// <summary>
        /// 配置DNP3规则
        /// </summary>
        /// <param name="dnp3rf">DNP3规则实例</param>
        /// <param name="add_delete">添加规则或者删除规则(值为true时添加规则)</param>
        bool ConfigDNP3Rules(DNP3RulesForm dnp3rf, bool add_delete);


        /// <summary>
        /// 添加白名单
        /// </summary>
        /// <param name="wl">白名单添加实例</param>
        /// <param name="add_delete">添加规则或者删除白名单(值为true时添加)</param>
        bool ConfigWhiteLists(WhiteLists wl, bool add_delete);

        /// <summary>
        /// 清除规则
        /// </summary>
        bool ClearAllRules();
    }
}
